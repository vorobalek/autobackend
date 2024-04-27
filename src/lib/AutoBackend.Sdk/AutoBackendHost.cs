using AutoBackend.Sdk.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoBackend.Sdk;

public class AutoBackendHost<T>
{
    public async Task RunAsync(string[] args, bool migrateRelationalOnStartup = false)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseUrls(
                    string.Format(
                        Constants.HostDefaultUrl,
                        Environment.GetEnvironmentVariable(Constants.PortEnvironmentVariable)));
                builder.UseStartup<StartupBoilerplate<T>>();
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