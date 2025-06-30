using CelestialStars_Domain;
using CelestialStars_Sql;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CelestialStars_Api.webhooks;

public static class WebhookController
{
    public static void MapWebhookEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var webhookGroup = routeBuilder.MapGroup("/webhooks")
            .RequireAuthorization();

        webhookGroup.MapTwitchWebhookEndpoints();

        webhookGroup.MapGet("/", GetWebhooks);
        webhookGroup.MapGet("/{webhookId:int}", GetWebhook);

        webhookGroup.MapPost("/", CreateNewWebhook);
    }

    #region Get Mappings

    private static async Task GetWebhooks(HttpContext httpContext, CelestialStarsDbContext dbContext)
    {
        var webhooks = await dbContext.Webhooks.ToListAsync();

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 200;

        var json = JsonConvert.SerializeObject(webhooks,
                                               new JsonSerializerSettings
                                               {
                                                   Formatting = Formatting.Indented,
                                                   ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                                               });

        await httpContext.Response.WriteAsync(json);
    }

    private static async Task GetWebhook(HttpContext httpContext, CelestialStarsDbContext dbContext, int webhookId)
    {
        var webhook = dbContext.Webhooks.FirstOrDefault(webhook => webhook.Id == webhookId);

        if (webhook is null)
        {
            httpContext.Response.StatusCode = 404;
            await httpContext.Response.WriteAsync($"Webhook with Id {webhookId} not found");
            return;
        }

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 200;

        var json = JsonConvert.SerializeObject(webhook,
                                               new JsonSerializerSettings
                                               {
                                                   Formatting = Formatting.Indented,
                                                   ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                                               });

        await httpContext.Response.WriteAsync(json);
    }

    #endregion Get Mappings

    private static async Task CreateNewWebhook(HttpContext httpContext, Webhook webhook)
    {
        httpContext.Response.StatusCode = 200;
        await httpContext.Response.WriteAsync("Jawollo");
    }
}