using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class InMemoryGenericDbContext(DbContextOptions<InMemoryGenericDbContext> options)
    : GenericDbContext(options), IGenericDbContext<InMemoryGenericDbContext>;