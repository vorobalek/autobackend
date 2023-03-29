using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithPrimaryKey<
    TEntity,
    TFilter,
    TKey
> : GenericController<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    private readonly IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> _genericRepository;

    public GenericControllerWithPrimaryKey(IGenericRepositoryWithPrimaryKey<TEntity, TFilter, TKey> genericRepository) :
        base(genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpGet("{key}")]
    public Task<ActionResult<GenericControllerResponse<TEntity?>>> GetByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(cancellationToken => _genericRepository.GetByPrimaryKeyAsync(key, cancellationToken));
    }

    [HttpPost("{key}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> InsertByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(
            cancellationToken => _genericRepository.InsertByPrimaryKeyAsync(key, entity, cancellationToken));
    }

    [HttpPut("{key}")]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> UpdateByPrimaryKeyAsync(
        [FromRoute] TKey key,
        [FromBody] TEntity entity)
    {
        return ProcessAsync(
            cancellationToken => _genericRepository.UpdateByPrimaryKeyAsync(key, entity, cancellationToken));
    }

    [HttpDelete("{key}")]
    public Task<ActionResult<GenericControllerResponse>> DeleteByPrimaryKeyAsync(
        [FromRoute] TKey key)
    {
        return ProcessAsync(cancellationToken => _genericRepository.DeleteByPrimaryKeyAsync(key, cancellationToken));
    }
}