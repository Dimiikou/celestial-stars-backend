using CelestialStars_Sql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CelestialStars_UnitTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions<CelestialStarsDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            const string connectionString = "Server=localhost;Database=testdb;User=root;Password=root;";
            // services.AddDbContext<CelestialStarsDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<CelestialStarsDbContext>(options => options.UseInMemoryDatabase("TestDb"));

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CelestialStarsDbContext>();
            db.Database.EnsureCreated();
        });
    }
}