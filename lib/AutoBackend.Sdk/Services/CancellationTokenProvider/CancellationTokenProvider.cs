using Microsoft.Extensions.Hosting;

namespace AutoBackend.Sdk.Services.CancellationTokenProvider;

internal sealed class CancellationTokenProvider : ICancellationTokenProvider
{
    private readonly CancellationTokenSource _tokenSource = new();

    public CancellationTokenProvider(IHostApplicationLifetime lifetime)
    {
        lifetime.ApplicationStopping.Register(_tokenSource.Cancel);
    }

    public CancellationToken GlobalCancellationToken => _tokenSource.Token;
}