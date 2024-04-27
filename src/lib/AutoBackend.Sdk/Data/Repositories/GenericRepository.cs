using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Exceptions.Data;
using AutoBackend.Sdk.Exceptions.Reflection;
using AutoBackend.Sdk.Filters;
using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data.Repositories;

internal class GenericRepository<TEntity, TFilter>(IGenericStorage<TEntity, TFilter> genericStorage)
    : IGenericRepository<TEntity, TFilter>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    public Task<TEntity[]> GetAllAsync(TFilter? filter, CancellationToken cancellationToken)
    {
        var query = genericStorage.GetQuery(filter);
        if (filter?.SkipCount is { } skipCountValue) query = query.Skip(skipCountValue);
        if (filter?.TakeCount is { } takeCountValue) query = query.Take(takeCountValue);
        return query.ToArrayAsync(cancellationToken);
    }

    public Task<long> GetCountAsync(TFilter? filter, CancellationToken cancellationToken)
    {
        return genericStorage.GetQuery(filter).LongCountAsync(cancellationToken);
    }

    protected async Task<TEntity?> GetByKeyInternalAsync(
        CancellationToken cancellationToken,
        params object[] keys)
    {
        return await genericStorage.FindAsync(keys, cancellationToken);
    }

    protected async Task<TEntity> CreateInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object[]? keys)
    {
        if (keys?.Any() ?? false)
        {
            ValidateEntityKeys(entity, keys);
            var current = await genericStorage.FindAsync(keys, cancellationToken);
            if (current is not null)
                throw new InconsistentDataException(
                    string.Format(
                        Constants.AnEntityWithTheSameKeyAlreadyExists,
                        string.Join(", ", keys.Select(key => key.ToString()))));
        }

        await genericStorage.AddAsync(entity, cancellationToken);
        await genericStorage.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task<TEntity> UpdateInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object[] keys)
    {
        ValidateEntityKeys(entity, keys);
        var current = await genericStorage.FindAsync(keys, cancellationToken);
        if (current is null)
            throw new NotFoundDataException(
                string.Format(
                    Constants.AnEntityWithTheGivenKeyDoesNotExist,
                    string.Join(", ", keys.Select(key => key.ToString()))));

        foreach (var entryProperty in genericStorage.Entry(current).Properties)
        {
            var entityProperty = typeof(TEntity).GetProperty(entryProperty.Metadata.Name);
            if (entityProperty is null)
                throw new NotFoundReflectionException(
                    string.Format(
                        Constants.UnableToFindAPropertyWithNameInObject,
                        entryProperty.Metadata.Name,
                        typeof(TEntity).Name));
            var newValue = entityProperty.GetValue(entity);
            entityProperty.SetValue(current, newValue);
        }

        genericStorage.Update(current);
        await genericStorage.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task DeleteInternalAsync(
        CancellationToken cancellationToken,
        params object[] keys)
    {
        var current = await genericStorage.FindAsync(keys, cancellationToken);
        if (current is null)
            throw new NotFoundDataException(
                string.Format(
                    Constants.AnEntityWithTheGivenKeyDoesNotExist,
                    string.Join(", ", keys.Select(key => key.ToString()))));

        genericStorage.Remove(current);
        await genericStorage.SaveChangesAsync(cancellationToken);
    }

    private void ValidateEntityKeys(
        TEntity entity,
        object[] keys)
    {
        var entry = genericStorage.Entry(entity);
        var entryProperties = entry
            .Properties
            .ToDictionary(
                x => x.Metadata.Name,
                x => x.CurrentValue);

        var entityPrimaryKey = entry.Metadata.FindPrimaryKey();
        if (entityPrimaryKey is null ||
            !keys
                .Select(key => key.GetType())
                .SequenceEqual(entityPrimaryKey
                    .Properties
                    .Select(key => key.ClrType)))
            throw new InconsistentDataException(Constants.TheEntityTypeKeySetDoesNotMatchTheGivenKeys);

        var entityKeyValues = entityPrimaryKey
            .Properties
            .Select(key => key.Name)
            .Select(keyName => entryProperties[keyName])
            .ToArray();

        if (!keys.SequenceEqual(entityKeyValues))
            throw new InconsistentDataException(
                string.Format(
                    Constants.TheEntityKeyValuesDoesNotMatchTheGivenKeyValues,
                    string.Join(", ", keys.Select(givenKeyValue => givenKeyValue.ToString())),
                    string.Join(", ", entityKeyValues.Select(entityKeyValue => entityKeyValue?.ToString()))));
    }
}