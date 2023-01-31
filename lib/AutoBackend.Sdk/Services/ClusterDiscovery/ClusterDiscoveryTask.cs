using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AutoBackend.Sdk.Services.ClusterDiscovery;

internal sealed class ClusterDiscoveryTask : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ClusterDiscoveryTask(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var clusterDiscovery = scope.ServiceProvider.GetRequiredService<IClusterDiscovery>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ClusterDiscoveryTask>>();
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(5000, cancellationToken);
            try
            {
                await clusterDiscovery.Discover(cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);
            }
        }
    }
}