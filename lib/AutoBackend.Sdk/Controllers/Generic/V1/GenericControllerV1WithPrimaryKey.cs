using AutoBackend.Sdk.Models.V1;
using AutoBackend.Sdk.Storage.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers.Generic.V1;

internal class GenericControllerV1WithPrimaryKey<
    TEntity,
    TFilter,
    TKey
> : GenericControllerV1<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithPrimaryKey<TEntity, TKey> _genericStorage;

    public GenericControllerV1WithPrimaryKey(IGenericStorageWithPrimaryKey<TEntity, TKey> genericStorage) : base(
        genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key}")]
    public Task<ActionResult<ApiResponseV1<TEntity?>>> GetByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByPrimaryKeyAsync(key, cancellationToken));
    }

    [HttpPost("{key}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> InsertByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(
            cancellationToken => _genericStorage.InsertByPrimaryKeyAsync(key, entity, cancellationToken));
    }

    [HttpPut("{key}")]
    public Task<ActionResult<ApiResponseV1<TEntity>>> UpdateByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(
            cancellationToken => _genericStorage.UpdateByPrimaryKeyAsync(key, entity, cancellationToken));
    }

    [HttpDelete("{key}")]
    public Task<ActionResult<ApiResponseV1>> DeleteByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByPrimaryKeyAsync(key, cancellationToken));
    }
}