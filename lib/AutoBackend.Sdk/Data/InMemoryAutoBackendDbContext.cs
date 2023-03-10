using Microsoft.EntityFrameworkCore;

namespace AutoBackend.Sdk.Data;

public sealed class InMemoryAutoBackendDbContext : AutoBackendDbContext<InMemoryAutoBackendDbContext>
{
    public InMemoryAutoBackendDbContext(DbContextOptions<InMemoryAutoBackendDbContext> options) : base(options)
    {
    }
}