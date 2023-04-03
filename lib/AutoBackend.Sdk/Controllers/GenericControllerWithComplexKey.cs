using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithComplexKey<
    TEntity,
    TFilter,
    TKey1,
    TKey2
> : GenericController<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
{
    private readonly IGenericRepositoryWithComplexKey<
        TEntity,
        TFilter,
        TKey1,
        TKey2
    > _genericRepository;

    public GenericControllerWithComplexKey(
        IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2
        > genericRepository) : base(genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpGet("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2)
    {
        return ProcessAsync(cancellationToken => _genericRepository.GetByComplexKeyAsync(
            key1,
            key2,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> CreateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericRepository.CreateByComplexKeyAsync(
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
        return ProcessAsync(cancellationToken => _genericRepository.UpdateByComplexKeyAsync(
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
        return ProcessAsync(cancellationToken => _genericRepository.DeleteByComplexKeyAsync(
            key1,
            key2,
            cancellationToken));
    }
}