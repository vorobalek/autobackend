using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace AutoBackend.Sdk.Data;

internal interface IGenericDbContext<TContext>
    where TContext : GenericDbContext, IGenericDbContext<TContext>
{
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