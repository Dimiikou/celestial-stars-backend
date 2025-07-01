using CelestialStars_Application;
using CelestialStars_Application.Users.login;
using CelestialStars_Application.users.register;
using CelestialStars_Application.webhooks.twitch.challengeRequest;
using CelestialStars_Application.webhooks.twitch.eventFired;
using CelestialStars_Application.webhooks.twitch.revocation;
using CelestialStars_Infrastructure;
using MediatR;
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
            options.UseMySql(connectionString, new MariaDbServerVersion(new Version(10, 6, 5)));
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
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