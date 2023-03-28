using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithComplexKey<
    TEntity,
    TKey1,
    TKey2,
    TKey3,
    TKey4,
    TKey5,
    TKey6
> : GenericController
    where TEntity : class
{
    private readonly IGenericStorageWithComplexKey<
        TEntity,
        TKey1,
        TKey2,
        TKey3,
        TKey4,
        TKey5,
        TKey6
    > _genericStorage;

    public GenericControllerWithComplexKey(
        IGenericStorageWithComplexKey<
            TEntity,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5,
            TKey6
        > genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpGet("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<GenericControllerResponse<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericStorage.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}/{key4}/{key5}/{key6}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromRoute] TKey6 key6)
    {
        return ProcessAsync(cancellationToken => _genericStorage.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            key6,
            cancellationToken));
    }
}