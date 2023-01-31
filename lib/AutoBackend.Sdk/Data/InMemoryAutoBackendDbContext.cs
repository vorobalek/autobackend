using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class InMemoryGenericDbContext : GenericDbContext, IGenericDbContext<InMemoryGenericDbContext>
{
    public InMemoryGenericDbContext(DbContextOptions<InMemoryGenericDbContext> options) : base(options)
    {
    }
}