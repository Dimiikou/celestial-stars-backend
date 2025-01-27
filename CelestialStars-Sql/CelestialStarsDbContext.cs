using CelestialStars_Domain;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Sql;

public class CelestialStarsDbContext : DbContext
{
    public CelestialStarsDbContext(DbContextOptions<CelestialStarsDbContext> options) : base(options)
    {

    }

    public DbSet<Webhook> Webhooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Webhook>(e =>
        {
            e.HasKey(entity => entity.Id);
        });
    }
}