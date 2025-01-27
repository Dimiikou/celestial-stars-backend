using CelestialStars_Api.accounting;
using CelestialStars_Api.webhooks;
using CelestialStars_Sql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<CelestialStarsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapWebhookEndpoints();
app.MapAccountApi();

app.Run();