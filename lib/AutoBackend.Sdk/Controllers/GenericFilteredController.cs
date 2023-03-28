using AutoBackend.Sdk.Models;
using AutoBackend.Sdk.Storage;
using Microsoft.AspNetCore.Mvc;

namespace AutoBackend.Sdk.Controllers;

internal sealed class GenericFilteredController<
    TEntity,
    TFilter
> : GenericController
    where TEntity : class
    where TFilter : class
{
    private readonly IGenericFilteredStorage<TEntity, TFilter> _genericStorage;

    public GenericFilteredController(IGenericFilteredStorage<TEntity, TFilter> genericStorage)
    {
        _genericStorage = genericStorage;
    }

    [HttpPost("filter")]
    public Task<ActionResult<GenericControllerResponse<TEntity[]>>> GetAllByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorage.GetAllByFilterAsync(filter, cancellationToken));
    }

    [HttpPost("filter/slice")]
    public Task<ActionResult<GenericControllerResponse<TEntity[]>>> GetByFilterSliceAsync(
        [FromBody] TFilter filter,
        [FromQuery(Name = "skip")] int? skipCount,
        [FromQuery(Name = "take")] int? takeCount)
    {
        return ProcessAsync(cancellationToken =>
            _genericStorage.GetSliceByFilterAsync(filter, skipCount, takeCount, cancellationToken));
    }

    [HttpPost("filter/count")]
    public Task<ActionResult<GenericControllerResponse<long>>> CountByFilterAsync(
        [FromBody] TFilter filter)
    {
        return ProcessAsync(cancellationToken => _genericStorage.CountByFilterAsync(filter, cancellationToken));
    }
}