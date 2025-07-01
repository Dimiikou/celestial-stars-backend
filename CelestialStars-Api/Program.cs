using System.Text;
using CelestialStars_Api;
using CelestialStars_Api.accounting;
using CelestialStars_Api.summonersQuiz;
using CelestialStars_Api.webhooks;
using CelestialStars_Application;
using CelestialStars_Domain;
using CelestialStars_Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("CelestialStars API wird gestartet...");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            ConfigureServices(builder.Services, builder.Environment, builder.Configuration);

            var app = builder.Build();

            if (!builder.Environment.IsEnvironment("Test"))
            {
                await MigrateDatabaseAsync(app);
            }

            ConfigureMiddleware(app, app.Environment);
            ConfigureEndpoints(app);

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "🔴 Schwerwiegender Fehler beim Starten der Anwendung.");
        }
        finally
        {
            Log.Information("🟡 CelestialStars API wird heruntergefahren.");
            await Log.CloseAndFlushAsync();
        }
    }

    private static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        // API Documentation
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        ConfigureSwagger(services);

        // Controllers
        services.AddControllers().AddNewtonsoftJson();
        services.AddProblemDetails();
        services.AddHealthChecks();

        // Auth
        ConfigureAuthentication(services, configuration);

        // Database
        services.AddDatabase(configuration, environment);

        // Application Services
        services.AddApplicationServices();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Aissa Rest Service",
                Version = "v1",
                Description = "API mit vollständiger Backend Funktionalität für den Aissa.dev Server",
                Contact = new OpenApiContact
                {
                    Name = "Aissa.dev Team",
                    Email = "contact@aissa.dev",
                    Url = new Uri("https://aissa.dev/")
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header mit Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

        if (jwtSettings is null)
        {
            throw new InvalidOperationException("JWT settings are not configured properly.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError("Authentication failed: {Message}", context.Exception.Message);
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
    }

    private static async Task MigrateDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CelestialStarsDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Starting database migration...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }

    private static void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ApiExceptionMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler();
            app.UseHsts();
        }

        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CelestialStars API v1");
            c.RoutePrefix = string.Empty;
            c.DocumentTitle = "Aissa API Documentation";
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        app.MapHealthChecks("/health");

        app.MapWebhookEndpoints();
        app.MapAccountApi();
        app.MapHighscoreApi();

        app.MapGet("/ping", () => "Ding Dong");
    }
}
