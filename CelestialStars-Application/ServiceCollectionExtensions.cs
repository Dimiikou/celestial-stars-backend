using CelestialStars_Application.Users.login;
using CelestialStars_Application.users.register;
using CelestialStars_Application.webhooks.twitch.challengeRequest;
using CelestialStars_Application.webhooks.twitch.eventFired;
using CelestialStars_Application.webhooks.twitch.revocation;
using CelestialStars_Domain;
using CelestialStars_Infrastructure;
using CelestialStars_Infrastructure.services;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CelestialStars_Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<CelestialStarsDbContext>(options =>
        {
            if (environment.IsEnvironment("Test"))
            {
                options.UseInMemoryDatabase("TestDb");
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, new MariaDbServerVersion(new Version(10, 6, 5)));
            }
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);
        services.AddScoped<AuthService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                                               typeof(LoginUserHandler).Assembly,
                                               typeof(RegisterUserHandler).Assembly,
                                               typeof(TwitchChallengeHandler).Assembly,
                                               typeof(TwitchEventFiredHandler).Assembly,
                                               typeof(TwitchRevocationHandler).Assembly
                                              );
        });

        return services;
    }
}