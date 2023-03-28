using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericControllerWithFilter<
    TEntity,
    TFilter
> : GenericController
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericStorageWithFilter<TEntity, TFilter> _genericStorageWithFilter;

    public GenericControllerWithFilter(IGenericStorageWithFilter<TEntity, TFilter> genericStorageWithFilter)
    {
        _genericStorageWithFilter = genericStorageWithFilter;
    }

    [HttpPost("filter")]
    public Task<ActionResult<GenericControllerResponse<TEntity[]>>> GetAllByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorageWithFilter.GetAllByFilterAsync(filter, cancellationToken));
    }

    [HttpPost("filter/slice")]
    public Task<ActionResult<GenericControllerResponse<TEntity[]>>> GetByFilterSliceAsync(
        [FromBody] TFilter filter,
        [FromQuery(Name = "skip")] int? skipCount,
        [FromQuery(Name = "take")] int? takeCount)
    {
        return ProcessAsync(cancellationToken =>
            _genericStorageWithFilter.GetSliceByFilterAsync(filter, skipCount, takeCount, cancellationToken));
    }

    [HttpPost("filter/count")]
    public Task<ActionResult<GenericControllerResponse<long>>> GetCountByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorageWithFilter.GetCountByFilterAsync(filter, cancellationToken));
    }
}