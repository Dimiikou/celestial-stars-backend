using CelestialStars_Application;
using CelestialStars_Application.webhooks.getWebHook;
using CelestialStars_Application.webhooks.getWebHooks;
using CelestialStars_Domain;
using MediatR;

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

    private static async Task GetWebhooks(HttpContext httpContext, ISender sender)
    {
        var webhooks = sender.Send(new GetWebhooksQuery());
        await httpContext.Response.WriteJsonAsync(webhooks);
    }

    private static async Task GetWebhook(HttpContext httpContext, int webhookId, ISender sender)
    {
        var webhook = await sender.Send(new GetWebhookQuery
        {
            Id = webhookId
        });

        await httpContext.Response.WriteJsonAsync(webhook);
    }

    #endregion Get Mappings

    private static async Task CreateNewWebhook(HttpContext httpContext, Webhook webhook)
    {
        httpContext.Response.StatusCode = 200;
        await httpContext.Response.WriteAsync("Jawollo");
    }
}