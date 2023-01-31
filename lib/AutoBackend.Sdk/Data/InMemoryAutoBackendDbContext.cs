using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class InMemoryAutoBackendDbContext : AutoBackendDbContext<InMemoryAutoBackendDbContext>
{
    public InMemoryAutoBackendDbContext(DbContextOptions<InMemoryAutoBackendDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase("InMemoryTemporary");
    }
}