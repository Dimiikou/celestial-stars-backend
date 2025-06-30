using System.Text;
using CelestialStars_Api.accounting;
using CelestialStars_Api.services;
using CelestialStars_Api.summonersQuiz;
using CelestialStars_Api.webhooks;
using CelestialStars_Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Environment, builder.Configuration);

        var app = builder.Build();

        ConfigureMiddleware(app, app.Environment);
        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddControllers().AddNewtonsoftJson();
        services.AddScoped<AuthService>();
        services.AddEndpointsApiExplorer();

        ConfigureSwagger(services);
        ConfigureAuthentication(services, configuration);

        services.AddDbContext<CelestialStarsDbContext>(options =>
        {
            if (!environment.IsEnvironment("Test"))
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        });
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Aissa Rest Service",
                Version = "v1",
                Description = "API mit vollständiger Backend Funktionalität für den Aissa.dev Server"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header mit Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
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
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        services.AddAuthorization();
    }

    private static void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "CelestialStars API v1");
            c.RoutePrefix = string.Empty;
        });

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        app.MapWebhookEndpoints();
        app.MapAccountApi();
        app.MapHighscoreApi();

        app.MapGet("/ping", () => "Ding Dong");
    }
}
