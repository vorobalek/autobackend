using System.Reflection;
using AutoBackend.Sdk.Attributes;
using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public class GenericDbContext : DbContext
{
#pragma warning disable CA2211
    private static Assembly[] _assemblies = Array.Empty<Assembly>();
#pragma warning restore CA2211

    protected GenericDbContext(DbContextOptions options) : base(options)
    {
    }

    internal static void SetAssemblies(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var assembly in _assemblies)
        {
            var candidateTypes = assembly
                .GetExportedTypes()
                .Where(e => e.GetCustomAttributes<GenericEntityAttribute>().Any());
            foreach (var candidateType in candidateTypes)
            {
                var attribute = candidateType.GetCustomAttribute<GenericEntityAttribute>()!;

                var entityBuilder = modelBuilder.Entity(candidateType);

                if (attribute.Keys.Any())
                {
                    entityBuilder.HasKey(attribute.Keys);
                }
                else
                {
                    var genericIdPropertyName = Constants.GenericIdPropertyName;
                    entityBuilder
                        .Property<int>(genericIdPropertyName)
                        .ValueGeneratedOnAdd();
                    entityBuilder.HasKey(genericIdPropertyName);
                }

                entityBuilder.ToTable(candidateType.Name, Constants.GenericDatabaseSchemaName);
            }
        }
    }
}