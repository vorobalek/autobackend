namespace AutoBackend.Sdk.Services.CancellationTokenProvider;

internal interface ICancellationTokenProvider
{
    CancellationToken GlobalCancellationToken { get; }
}