namespace AutoBackend.Sdk.Filters;

internal interface IGenericFilter
{
    int? SkipCount { get; }
    int? TakeCount { get; }
}