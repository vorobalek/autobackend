using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Exceptions.Api;
using AutoBackend.Sdk.Filters;
using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data.Repositories;

internal class GenericRepository<TEntity, TFilter> : IGenericRepository<TEntity, TFilter>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    private readonly IGenericStorage<TEntity, TFilter> _genericStorage;

    public GenericRepository(IGenericStorage<TEntity, TFilter> genericStorage)
    {
        _genericStorage = genericStorage;
    }

    public Task<TEntity[]> GetAllAsync(TFilter? filter, CancellationToken cancellationToken)
    {
        var query = _genericStorage.GetQuery(filter);
        if (filter?.SkipCount is { } skipCountValue) query = query.Skip(skipCountValue);
        if (filter?.TakeCount is { } takeCountValue) query = query.Take(takeCountValue);
        return query.ToArrayAsync(cancellationToken);
    }

    public Task<long> GetCountAsync(TFilter? filter, CancellationToken cancellationToken)
    {
        return _genericStorage.GetQuery(filter).LongCountAsync(cancellationToken);
    }

    protected async Task<TEntity?> GetByKeyInternalAsync(
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        return await _genericStorage.FindAsync(keys, cancellationToken);
    }

    protected async Task<TEntity> InsertInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        if (keys?.Any() ?? false)
        {
            var current = await _genericStorage.FindAsync(keys, cancellationToken);
            if (current is not null)
                throw new BadRequestApiException(
                    $"Entity with same key already exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())}).");
        }

        await _genericStorage.AddAsync(entity, cancellationToken);
        await _genericStorage.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task<TEntity> UpdateInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _genericStorage.FindAsync(keys, cancellationToken);
        if (current is null)
            throw new BadRequestApiException(
                $"Entity with given key does not exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())}).");

        foreach (var entryProperty in _genericStorage.Entry(current).Properties)
        {
            var entityProperty = typeof(TEntity).GetProperty(entryProperty.Metadata.Name);
            if (entityProperty is null)
                throw new NotFoundApiException(
                    $"Unable to find property with name {entryProperty.Metadata.Name} in object {typeof(TEntity).Name}.");
            var newValue = entityProperty.GetValue(entity);
            entityProperty.SetValue(current, newValue);
        }

        _genericStorage.Update(current);
        await _genericStorage.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task DeleteInternalAsync(
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _genericStorage.FindAsync(keys, cancellationToken);
        if (current is null)
            throw new NotFoundApiException(
                $"Entity with given key does not exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())}).");

        _genericStorage.Remove(current);
        await _genericStorage.SaveChangesAsync(cancellationToken);
    }
}