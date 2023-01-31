using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class PostgresGenericDbContext : GenericDbContext, IGenericDbContext<PostgresGenericDbContext>
{
    public PostgresGenericDbContext(DbContextOptions<PostgresGenericDbContext> options) : base(options)
    {
    }
}