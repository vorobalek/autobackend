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
    TKey3,
    TKey4,
    TKey5
> : GenericController<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
    where TKey1 : notnull
    where TKey2 : notnull
    where TKey3 : notnull
    where TKey4 : notnull
    where TKey5 : notnull
{
    private readonly IGenericRepositoryWithComplexKey<
        TEntity,
        TFilter,
        TKey1,
        TKey2,
        TKey3,
        TKey4,
        TKey5
    > _genericRepository;

    public GenericControllerWithComplexKey(
        IGenericRepositoryWithComplexKey<
            TEntity,
            TFilter,
            TKey1,
            TKey2,
            TKey3,
            TKey4,
            TKey5
        > genericRepository) : base(genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpGet("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<GenericControllerResponse<TEntity?>>> GetByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5)
    {
        return ProcessAsync(cancellationToken => _genericRepository.GetByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            cancellationToken));
    }

    [HttpPost("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> CreateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericRepository.CreateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            entity,
            cancellationToken));
    }

    [HttpPut("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> UpdateByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(cancellationToken => _genericRepository.UpdateByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            entity,
            cancellationToken));
    }

    [HttpDelete("{key1}/{key2}/{key3}/{key4}/{key5}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByComplexKeyAsync(
        [FromRoute] TKey1 key1,
        [FromRoute] TKey2 key2,
        [FromRoute] TKey3 key3,
        [FromRoute] TKey4 key4,
        [FromRoute] TKey5 key5)
    {
        return ProcessAsync(cancellationToken => _genericRepository.DeleteByComplexKeyAsync(
            key1,
            key2,
            key3,
            key4,
            key5,
            cancellationToken));
    }
}