using CelestialStars_Domain;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Sql;

public class CelestialStarsDbContext : DbContext
{
    public CelestialStarsDbContext(DbContextOptions<CelestialStarsDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Webhook> Webhooks { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<Highscore> Highscores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(entity => entity.Id);
        });

        modelBuilder.Entity<Webhook>(e =>
        {
            e.HasKey(entity => entity.Id);
        });

        modelBuilder.Entity<Highscore>(e =>
        {
            e.HasKey(entity => entity.Id);
        });

        modelBuilder.Entity<LogEntry>(e =>
        {
            e.HasKey(entity => entity.Id);
            e.HasOne(entity => entity.Webhook)
                .WithMany(webhook => webhook.LogEntries)
                .HasForeignKey(entity => entity.WebhookId);
        });
    }
}