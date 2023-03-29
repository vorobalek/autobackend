using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2,
    TKey3
> : GenericController<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    private readonly IGenericRepositoryWithComplexKey<
        TEntity,
        TFilter,
        TKey1,
        TKey2,
        TKey3
    > _genericRepository;

    public GenericControllerWithComplexKey(
        IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3
        > genericRepository) : base(genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpGet("{key1}/{key2}/{key3}")]
    public Task<ActionResult<GenericControllerResponse<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3)
    {
        return ProcessAsync(cancellationToken => _genericRepository.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> InsertByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericRepository.InsertByComplexKeyAsync(
            key1,
            key2,
            key3,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericRepository.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3)
    {
        return ProcessAsync(cancellationToken => _genericRepository.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            cancellationToken));
    }
}