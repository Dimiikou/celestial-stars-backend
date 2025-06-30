using CelestialStars_Api.services;
using CelestialStars_Sql;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<CelestialStarsDbContext>(options =>
        {
            if (environment.IsEnvironment("Test"))
            {
                return;
            }

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                                                maxRetryCount: 5,
                                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                                errorNumbersToAdd: null);
            });
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();

        return services;
    }
}