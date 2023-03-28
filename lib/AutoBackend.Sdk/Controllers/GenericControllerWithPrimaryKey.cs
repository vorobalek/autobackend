using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithPrimaryKey<
    TEntity,
    TKey
> : GenericController
    where TEntity : class
{
    private readonly IGenericStorageWithPrimaryKey<TEntity, TKey> _genericStorage;

    public GenericControllerWithPrimaryKey(IGenericStorageWithPrimaryKey<TEntity, TKey> genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key}")]
    public Task<ActionResult<GenericControllerResponse<TEntity?>>> GetByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByPrimaryKeyAsync(key, cancellationToken));
    }

    [HttpPost("{key}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> InsertByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(
            cancellationToken => _genericStorage.InsertByPrimaryKeyAsync(key, entity, cancellationToken));
    }

    [HttpPut("{key}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> UpdateByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(
            cancellationToken => _genericStorage.UpdateByPrimaryKeyAsync(key, entity, cancellationToken));
    }

    [HttpDelete("{key}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByPrimaryKeyAsync(key, cancellationToken));
    }
}