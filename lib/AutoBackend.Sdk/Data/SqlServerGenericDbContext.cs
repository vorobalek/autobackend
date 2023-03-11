using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class SqlServerGenericDbContext : GenericDbContext, IGenericDbContext<SqlServerGenericDbContext>
{
    public SqlServerGenericDbContext(DbContextOptions<SqlServerGenericDbContext> options) : base(options)
    {
    }
}