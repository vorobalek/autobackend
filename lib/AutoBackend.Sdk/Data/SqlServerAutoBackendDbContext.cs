using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class SqlServerAutoBackendDbContext : AutoBackendDbContext<SqlServerAutoBackendDbContext>
{
    public SqlServerAutoBackendDbContext(DbContextOptions<SqlServerAutoBackendDbContext> options) : base(options)
    {
    }
}