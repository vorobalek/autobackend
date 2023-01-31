using System.Reflection;
using AutoBackend.Sdk.Attributes.GenericEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Data;

public abstract class AutoBackendDbContext : DbContext
{
#pragma warning disable CA2211
    protected static Assembly[] Assemblies = Array.Empty<Assembly>();
#pragma warning restore CA2211

    protected AutoBackendDbContext(DbContextOptions options) : base(options)
    {
    }

    internal static void SetAssemblies(params Assembly[] assemblies)
    {
        Assemblies = assemblies;
    }
}

public abstract class AutoBackendDbContext<TContext> : AutoBackendDbContext
    where TContext : AutoBackendDbContext<TContext>
{
    private const string GenericEntitySchema = "generic";

    protected AutoBackendDbContext(DbContextOptions<TContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var assembly in Assemblies)
        {
            var candidateTypes = assembly
                .GetExportedTypes()
                .Where(e => e.GetCustomAttributes<GenericEntityAttribute>().Any());
            foreach (var candidateType in candidateTypes)
            {
                var attribute = candidateType.GetCustomAttribute<GenericEntityAttribute>()!;

                var entityBuilder = modelBuilder.Entity(candidateType);

                if (attribute.Keys.Any())
                    entityBuilder.HasKey(attribute.Keys);
                else
                    entityBuilder.HasNoKey();

                entityBuilder.ToTable(candidateType.Name, GenericEntitySchema);
            }
        }
    }

    internal abstract class DesignTimeFactory : IDesignTimeDbContextFactory<TContext>
    {
        private static TContext Context { get; set; } = null!;

        public TContext CreateDbContext(string[] args)
        {
            return Context;
        }

        public static void Initialize(IServiceCollection services)
        {
            Context = services.BuildServiceProvider().GetRequiredService<TContext>();
        }
    }
}