using AutoBackend.Sdk.Data;
using AutoBackend.Sdk.Exceptions.Api;
using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Storage;

internal class GenericStorage<TEntity> : IGenericStorage<TEntity> where TEntity : class
{
    private readonly GenericDbContext _db;

    public GenericStorage(GenericDbContext db)
    {
        _db = db;
    }

    public Task<TEntity[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _db.Set<TEntity>().ToArrayAsync(cancellationToken);
    }

    public Task<TEntity[]> GetSliceAsync(int? skipCount, int? takeCount, CancellationToken cancellationToken = default)
    {
        var query = _db.Set<TEntity>().AsQueryable();
        if (skipCount is { } skipCountValue) query = query.Skip(skipCountValue);
        if (takeCount is { } takeCountValue) query = query.Take(takeCountValue);
        return query.ToArrayAsync(cancellationToken);
    }

    public Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return _db.Set<TEntity>().LongCountAsync(cancellationToken);
    }

    protected async Task<TEntity?> GetByKeyInternalAsync(
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        return await _db.Set<TEntity>().FindAsync(keys, cancellationToken);
    }

    protected async Task<TEntity> InsertInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _db.Set<TEntity>().FindAsync(keys);
        if (current is not null)
            throw new BadRequestApiException(
                $"Entity with same key already exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())}).");

        await _db.Set<TEntity>().AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task<TEntity> UpdateInternalAsync(
        TEntity entity,
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _db.Set<TEntity>().FindAsync(keys);
        if (current is null)
            throw new BadRequestApiException(
                $"Entity with given key does not exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())}).");

        foreach (var entryProperty in _db.Entry(current).Properties)
        {
            var entityProperty = typeof(TEntity).GetProperty(entryProperty.Metadata.Name);
            if (entityProperty is null)
                throw new NotFoundApiException(
                    $"Unable to find property with name {entryProperty.Metadata.Name} in object {typeof(TEntity).Name}.");
            var newValue = entityProperty.GetValue(entity);
            entityProperty.SetValue(current, newValue);
        }

        _db.Set<TEntity>().Update(current);
        await _db.SaveChangesAsync(cancellationToken);
        return entity;
    }

    protected async Task DeleteInternalAsync(
        CancellationToken cancellationToken,
        params object?[]? keys)
    {
        var current = await _db.Set<TEntity>().FindAsync(keys);
        if (current is null)
            throw new NotFoundApiException(
                $"Entity with given key does not exists ({string.Join(',', keys?.Select(key => key?.ToString()) ?? Array.Empty<string?>())}).");

        _db.Set<TEntity>().Remove(current);
        await _db.SaveChangesAsync(cancellationToken);
    }
}