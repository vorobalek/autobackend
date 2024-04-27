using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class PostgresGenericDbContext(DbContextOptions<PostgresGenericDbContext> options)
    : GenericDbContext(options), IGenericDbContext<PostgresGenericDbContext>;