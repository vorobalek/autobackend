namespace AutoBackend.Sdk.Controllers.Generic.V2;

internal sealed class GenericControllerV2<
    TEntity,
    TFilter
> : GenericController
    where TEntity : class
    where TFilter : class
{
}