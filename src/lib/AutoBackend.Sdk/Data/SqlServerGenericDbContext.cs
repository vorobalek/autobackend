using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class SqlServerGenericDbContext(DbContextOptions<SqlServerGenericDbContext> options)
    : GenericDbContext(options), IGenericDbContext<SqlServerGenericDbContext>;