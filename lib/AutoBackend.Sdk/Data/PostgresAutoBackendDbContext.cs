using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class PostgresAutoBackendDbContext : AutoBackendDbContext<PostgresAutoBackendDbContext>
{
    public PostgresAutoBackendDbContext(DbContextOptions<PostgresAutoBackendDbContext> options) : base(options)
    {
    }
}