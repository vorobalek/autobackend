using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Filters;
using AutoBackend.Sdk.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithNoKey<
    TEntity,
    TFilter
> : GenericController<
    TEntity,
    TFilter
>
    where TEntity : class
    where TFilter : class, IGenericFilter
{
    private readonly IGenericRepositoryWithNoKey<TEntity, TFilter> _genericRepository;

    public GenericControllerWithNoKey(IGenericRepositoryWithNoKey<TEntity, TFilter> genericRepository) : base(
        genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpPost]
    public Task<ActionResult<GenericControllerResponse<TEntity>>> CreateAsync(
        [FromBody] TEntity entity)
    {
        return ProcessAsync(
            cancellationToken => _genericRepository.CreateAsync(entity, cancellationToken));
    }
}