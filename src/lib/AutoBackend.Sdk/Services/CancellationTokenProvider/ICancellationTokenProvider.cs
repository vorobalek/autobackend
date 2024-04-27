namespace AutoBackend.Sdk.Services.CancellationTokenProvider;

internal interface ICancellationTokenProvider
{
    CancellationToken GlobalToken { get; }
}