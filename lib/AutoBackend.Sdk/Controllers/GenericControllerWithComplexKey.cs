using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithComplexKey<
    TEntity,
    TKey1,
    TKey2
> : GenericController
    where TEntity : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2
    > _genericStorage;

    public GenericControllerWithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2
        > genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            cancellationToken));
    }
}