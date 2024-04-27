using Microsoft.Extensions.Hosting;

namespace AutoBackend.Sdk.Services.CancellationTokenProvider;

internal sealed class CancellationTokenProvider : ICancellationTokenProvider
{
    private readonly CancellationTokenSource _globalCancellationTokenSource = new();

    public CancellationTokenProvider(IHostApplicationLifetime lifetime)
    {
        lifetime.ApplicationStopping.Register(_globalCancellationTokenSource.Cancel);
    }

    public CancellationToken GlobalToken => _globalCancellationTokenSource.Token;
}