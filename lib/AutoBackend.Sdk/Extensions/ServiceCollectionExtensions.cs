using AutoBackend.Sdk.Configuration;
using AutoBackend.Sdk.Controllers;
using AutoBackend.Sdk.Controllers.Infrastructure;
using AutoBackend.Sdk.Data;
using AutoBackend.Sdk.Enums;
using AutoBackend.Sdk.Exceptions;
using AutoBackend.Sdk.NSwag;
using AutoBackend.Sdk.Services.ClusterDiscovery;
using AutoBackend.Sdk.Services.DateTimeProvider;
using AutoBackend.Sdk.Services.ExceptionHandler;
using AutoBackend.Sdk.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;

namespace AutoBackend.Sdk.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAutoBackend<TProgram>(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddGenericControllers<TProgram>()
            .AddGenericControllersSwagger(typeof(TProgram).Assembly.FullName!)
            .AddGenericDbContext<TProgram>(configuration)
            .AddGenericStorage()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddScoped<IExceptionHandlerFactory, ExceptionHandlerFactory>()
            .AddScoped<IClusterDiscovery, ClusterDiscovery>()
            .AddHostedService<ClusterDiscoveryTask>();
    }

    private static IServiceCollection AddGenericControllers<TProgram>(
        this IServiceCollection services)
    {
        services
            .AddControllers(o => o
                .Conventions
                .Add(new GenericControllerRouteConvention()))
            .ConfigureApplicationPartManager(m => m
                .FeatureProviders
                .Add(new GenericControllerTypeFeatureProvider(typeof(AutoBackendHost<>).Assembly,
                    typeof(TProgram).Assembly)));

        return services;
    }

    private static IServiceCollection AddGenericControllersSwagger(
        this IServiceCollection services,
        string swaggerTitle)
    {
        services.AddSwaggerDocument(settings =>
        {
            settings.Title = swaggerTitle;
            settings.DocumentName = GenericController.Version;
            settings.PostProcess = document => { document.Info.Version = GenericController.Version; };
            settings.ApiGroupNames = new[] { GenericController.Version };
            settings.TypeNameGenerator = new NSwagTypeNameGenerator();
            settings.SchemaNameGenerator = new NSwagSchemaNameGenerator();

            settings.OperationProcessors.Replace<OperationTagsProcessor>(new NSwagOperationTagsProcessor());
        });

        return services;
    }

    private static IServiceCollection AddGenericStorage(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericStorage<>), typeof(GenericStorage<>));
        services.AddScoped(typeof(IGenericStorageWithPrimaryKey<,>), typeof(GenericStorageWithPrimaryKey<,>));
        services.AddScoped(typeof(IGenericStorageWithComplexKey<,,>), typeof(GenericStorageWithComplexKey<,,>));
        services.AddScoped(typeof(IGenericStorageWithComplexKey<,,,>), typeof(GenericStorageWithComplexKey<,,,>));
        services.AddScoped(typeof(IGenericStorageWithComplexKey<,,,,>), typeof(GenericStorageWithComplexKey<,,,,>));
        services.AddScoped(typeof(IGenericStorageWithComplexKey<,,,,,>), typeof(GenericStorageWithComplexKey<,,,,,>));
        services.AddScoped(typeof(IGenericStorageWithComplexKey<,,,,,,>), typeof(GenericStorageWithComplexKey<,,,,,,>));
        services.AddScoped(typeof(IGenericStorageWithComplexKey<,,,,,,,>),
            typeof(GenericStorageWithComplexKey<,,,,,,,>));
        services.AddScoped(typeof(IGenericStorageWithComplexKey<,,,,,,,,>),
            typeof(GenericStorageWithComplexKey<,,,,,,,,>));

        return services;
    }

    private static IServiceCollection AddGenericDbContext<TProgram>(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        const string databasesConfigurationSectionName = "Database";
        const string autoBackendInMemoryDatabaseName = "AutoBackendInMemoryDatabase";
        GenericDbContext.SetAssemblies(typeof(AutoBackendHost<>).Assembly, typeof(TProgram).Assembly);

        var databasesConfiguration = configuration
            .GetSection(databasesConfigurationSectionName)
            .Get<DatabaseConfiguration?>();

        if (databasesConfiguration is null)
            return services.AddSpecificGenericDbContext<InMemoryGenericDbContext>(true, builder =>
            {
                builder
                    .UseInMemoryDatabase(autoBackendInMemoryDatabaseName);
            });

        var isPrimaryConfigured = false;
        foreach (var databaseConfiguration in databasesConfiguration.Providers)
        {
            var isPrimary = databaseConfiguration.Key == databasesConfiguration.PrimaryProvider;
            switch (databaseConfiguration.Key)
            {
                case DatabaseProviderType.InMemory:
                    services.AddSpecificGenericDbContext<InMemoryGenericDbContext>(isPrimary, builder =>
                    {
                        builder
                            .UseInMemoryDatabase(databaseConfiguration.Value);
                    });
                    isPrimaryConfigured |= isPrimary;
                    break;
                case DatabaseProviderType.SqlServer:
                    services
                        .AddSpecificGenericDbContext<SqlServerGenericDbContext>(isPrimary, builder =>
                        {
                            builder
                                .UseSqlServer(
                                    databaseConfiguration.Value,
                                    optionsBuilder =>
                                    {
                                        optionsBuilder
                                            .MigrationsAssembly(typeof(TProgram).Assembly.FullName);
                                    });
                        });
                    isPrimaryConfigured |= isPrimary;
                    break;
                case DatabaseProviderType.Postgres:
                    services.AddSpecificGenericDbContext<PostgresGenericDbContext>(isPrimary, builder =>
                    {
                        builder
                            .UseNpgsql(
                                databaseConfiguration.Value,
                                optionsBuilder =>
                                {
                                    optionsBuilder
                                        .MigrationsAssembly(typeof(TProgram).Assembly.FullName);
                                });
                    });
                    isPrimaryConfigured |= isPrimary;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseConfiguration.Key));
            }
        }

        if (!isPrimaryConfigured)
            throw new AutoBackendException("No one primary database provider was configured");

        return services;
    }

    private static IServiceCollection AddSpecificGenericDbContext<TContext>(
        this IServiceCollection services,
        bool isPrimary,
        Action<DbContextOptionsBuilder>? action = null)
        where TContext : GenericDbContext, IGenericDbContext<TContext>
    {
        services.AddDbContext<TContext>(action);
        IGenericDbContext<TContext>.DesignTimeFactory.Initialize(services);

        if (isPrimary) services.AddScoped<GenericDbContext, TContext>();

        return services;
    }
}