using AutoBackend.Sdk.Data;
using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Middleware.ApiExceptionHandler;
using AutoBackend.Sdk.Middleware.RequestTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoBackend.Sdk;

public class AutoBackendHost<T>
{
    private const string HealthCheckUrl = "/__health";

    public async Task RunAsync(string[] args, bool migrateRelationalOnStartup = false)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder
                    .UseUrls(
                        string.Format(
                            Constants.HostDefaultUrl,
                            Environment.GetEnvironmentVariable(Constants.PortEnvironmentVariable)))
                    .ConfigureServices((context, services) =>
                    {
                        services
                            .AddAutoBackend<T>(context.Configuration)
                            .AddHealthChecks();
                    })
                    .Configure(app =>
                    {
                        app
                            .UseApiExceptionHandler()
                            .UseRequestTimestamp()
                            .UseStaticFiles()
                            .UseOpenApi()
                            .UseSwaggerUi()
                            .UseHealthChecks(HealthCheckUrl)
                            .UseRouting()
                            .UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                                endpoints.MapHealthChecks(HealthCheckUrl, new HealthCheckOptions
                                {
                                    AllowCachingResponses = false
                                });
                                endpoints.MapGraphQL().AllowAnonymous();
                            });
                    });
            })
            .Build();

        await using var asyncScope = host
            .Services
            .CreateAsyncScope();

        var database = asyncScope
            .ServiceProvider
            .GetRequiredService<GenericDbContext>()
            .Database;

        if (database.IsRelational() && migrateRelationalOnStartup)
            await database.MigrateAsync();

        await host.RunAsync();
    }
}