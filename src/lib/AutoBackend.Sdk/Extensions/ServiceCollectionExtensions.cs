using AutoBackend.Sdk.Builders;
using AutoBackend.Sdk.Configuration;
using AutoBackend.Sdk.Controllers.Infrastructure;
using AutoBackend.Sdk.Data;
using AutoBackend.Sdk.Data.Mappers;
using AutoBackend.Sdk.Data.Repositories;
using AutoBackend.Sdk.Data.Storage;
using AutoBackend.Sdk.Enums;
using AutoBackend.Sdk.Exceptions.Configuration;
using AutoBackend.Sdk.NSwag;
using AutoBackend.Sdk.Services.CancellationTokenProvider;
using AutoBackend.Sdk.Services.DateTimeProvider;
using AutoBackend.Sdk.Services.EntityMetadataProvider;
using AutoBackend.Sdk.Services.ExceptionHandler;
using AutoBackend.Sdk.Services.PermissionValidator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;

namespace AutoBackend.Sdk.Extensions;

internal static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAutoBackend<TProgram>(IConfiguration configuration)
        {
            return services
                .AddGenericRequestResponse()
                .AddGenericControllers<TProgram>()
                .AddGenericGql<TProgram>()
                .AddGenericSwagger(typeof(TProgram).Assembly.FullName!)
                .AddGenericDbContext<TProgram>(configuration)
                .AddGenericStorage()
                .AddGenericPermissions(configuration)
                .AddInternalServices();
        }

        private IServiceCollection AddGenericControllers<TProgram>()
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

        private IServiceCollection AddGenericRequestResponse()
        {
            services.AddSingleton<IMapperExpressionsCache, MapperExpressionsCache>();
            services.AddScoped<IGenericRequestMapper, GenericRequestMapper>();
            services.AddScoped<IGenericResponseMapper, GenericResponseMapper>();
            return services;
        }

        private IServiceCollection AddGenericGql<TProgram>()
        {
            var rootQueryType = GenericGqlQueryTypeBuilder.Build(
                typeof(AutoBackendHost<>).Assembly,
                typeof(TProgram).Assembly);

            var rootMutationType = GenericGqlMutationTypeBuilder.Build(
                typeof(AutoBackendHost<>).Assembly,
                typeof(TProgram).Assembly);

            services
                .AddGraphQLServer()
                .AddQueryType(rootQueryType)
                .AddMutationType(rootMutationType)
                .AddProjections()
                .UseExceptions()
                .UseTimeout()
                .UseDocumentCache()
                .UseDocumentParser()
                .UseDocumentValidation()
                .UseOperationCache()
                .UseOperationResolver()
                .UseOperationVariableCoercion()
                .UseOperationExecution()
                .AddType(new TimeSpanType(TimeSpanFormat.DotNet));

            return services;
        }

        private IServiceCollection AddGenericSwagger(string swaggerTitle)
        {
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = swaggerTitle;
                settings.DocumentName = Constants.ApiGroupName;
                settings.PostProcess = document => { document.Info.Version = Constants.ApiVersion; };
                settings.ApiGroupNames = [Constants.ApiGroupName];
                settings.SchemaSettings.TypeNameGenerator = new NSwagTypeNameGenerator();
                settings.SchemaSettings.SchemaNameGenerator = new NSwagSchemaNameGenerator();

                settings.OperationProcessors.Replace<OperationTagsProcessor>(new NSwagOperationTagsProcessor());
            });

            return services;
        }

        private IServiceCollection AddGenericDbContext<TProgram>(IConfiguration configuration)
        {
            GenericDbContext.SetAssemblies(typeof(AutoBackendHost<>).Assembly, typeof(TProgram).Assembly);

            var databasesConfiguration = configuration
                .GetSection(Constants.DatabaseConfigurationSectionName)
                .Get<DatabaseConfiguration?>();

            if (databasesConfiguration is null)
                return services.AddSpecificGenericDbContext<InMemoryGenericDbContext>(true, builder =>
                {
                    builder
                        .UseInMemoryDatabase(Constants.GenericInMemoryDatabaseName);
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
                throw new InvalidConfigurationException(Constants.NoDatabaseProviderHasBeenChosenAsAPrimaryOne);

            return services;
        }

        private IServiceCollection AddSpecificGenericDbContext<TContext>(bool isPrimary,
            Action<DbContextOptionsBuilder>? action = null)
            where TContext : GenericDbContext, IGenericDbContext<TContext>
        {
            services.AddDbContext<TContext>(action);
            IGenericDbContext<TContext>.DesignTimeFactory.Initialize(services);

            if (isPrimary) services.AddScoped<GenericDbContext, TContext>();

            return services;
        }

        private IServiceCollection AddGenericStorage()
        {
            services.AddScoped(typeof(IGenericStorage<,>), typeof(GenericStorage<,>));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped(typeof(IGenericRepositoryWithNoKey<,>), typeof(GenericRepositoryWithNoKey<,>));
            services.AddScoped(typeof(IGenericRepositoryWithPrimaryKey<,,>), typeof(GenericRepositoryWithPrimaryKey<,,>));
            services.AddScoped(typeof(IGenericRepositoryWithComplexKey<,,,>), typeof(GenericRepositoryWithComplexKey<,,,>));
            services.AddScoped(typeof(IGenericRepositoryWithComplexKey<,,,,>),
                typeof(GenericRepositoryWithComplexKey<,,,,>));
            services.AddScoped(typeof(IGenericRepositoryWithComplexKey<,,,,,>),
                typeof(GenericRepositoryWithComplexKey<,,,,,>));
            services.AddScoped(typeof(IGenericRepositoryWithComplexKey<,,,,,,>),
                typeof(GenericRepositoryWithComplexKey<,,,,,,>));
            services.AddScoped(typeof(IGenericRepositoryWithComplexKey<,,,,,,,>),
                typeof(GenericRepositoryWithComplexKey<,,,,,,,>));
            services.AddScoped(typeof(IGenericRepositoryWithComplexKey<,,,,,,,,>),
                typeof(GenericRepositoryWithComplexKey<,,,,,,,,>));
            services.AddScoped(typeof(IGenericRepositoryWithComplexKey<,,,,,,,,,>),
                typeof(GenericRepositoryWithComplexKey<,,,,,,,,,>));

            return services;
        }

        private IServiceCollection AddGenericPermissions(IConfiguration configuration)
        {
            services.Configure<JwtConfiguration>(
                configuration
                    .GetSection(Constants.JwtConfigurationSectionName));

            services
                .AddScoped<IPermissionValidator, PermissionValidator>();
            
            return services;
        }

        private IServiceCollection AddInternalServices()
        {
            return services
                .AddSingleton<IDateTimeProvider, DateTimeProvider>()
                .AddSingleton<ICancellationTokenProvider, CancellationTokenProvider>()
                .AddSingleton<IEntityMetadataProvider, EntityMetadataProvider>()
                .AddScoped<IExceptionHandlerFactory, ExceptionHandlerFactory>()
                .AddHttpContextAccessor();
        }
    }
}