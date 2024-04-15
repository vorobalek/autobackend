using AutoBackend.Sdk.Extensions;
using AutoBackend.Sdk.Middleware.ApiExceptionHandler;
using AutoBackend.Sdk.Middleware.RequestTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk;

internal sealed class StartupBoilerplate<TProgram>
{
    private const string HealthCheckUrl = "/__health";
    private readonly IConfiguration _configuration;

    public StartupBoilerplate(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddAutoBackend<TProgram>(_configuration)
            .AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
            .UseApiExceptionHandler()
            .UseRequestTimestamp()
            .UseStaticFiles()
            .UseOpenApi()
            .UseSwaggerUi()
            .UseHealthChecks(HealthCheckUrl)
            .UseRouting()
            .UseEndpoints(builder =>
            {
                builder.MapControllers();
                builder.MapHealthChecks(HealthCheckUrl, new HealthCheckOptions
                {
                    AllowCachingResponses = false
                });
                builder.MapClusterDiscovery();
                builder.MapGraphQL().AllowAnonymous();
            });
    }
}