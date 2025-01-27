using System.Diagnostics;
using CelestialStars_Api.services;
using CelestialStars_Domain;
using CelestialStars_Sql;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Api.webhooks;

public static class TwitchWebhookContoller
{
    public static void MapTwitchWebhookEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/twitch", ProcessWebhook);
    }

    private static async Task ProcessWebhook(HttpContext httpContext, CelestialStarsDbContext dbContext)
    {
        if (!httpContext.Request.Headers.TryGetValue("twitch-eventsub-message-type", out var eventSubMessageType))
        {
            httpContext.Response.StatusCode = 403;
            await httpContext.Response.WriteAsync("Signature Header not valid");
            return;
        }

        var webhook = dbContext.Webhooks.FirstOrDefault(webhook => webhook.SubscribedEvent.Equals(httpContext.Request.Headers[""]));

        if (webhook is null)
        {
            httpContext.Response.StatusCode = 404;
            await httpContext.Response.WriteAsync("Webhook nicht gefunden");
            return;
        }

        switch (eventSubMessageType)
        {
            case "webhook_callback_verification":
                await ProcessChallengeRequest(httpContext);
                break;
            case "revocation":
                await ProcessRevocationRequest(httpContext, dbContext);
                break;
            default:
                await ProcessWebhookAction(webhook);
                break;
        }
    }

    private static async Task ProcessChallengeRequest(HttpContext httpContext)
    {
        var challengeString = await JsonHelper.GetJsonPropertyValueAsync(httpContext, "challenge");
        httpContext.Response.StatusCode = 200;
        await httpContext.Response.WriteAsync(challengeString);
    }

    private static async Task ProcessRevocationRequest(HttpContext httpContext, CelestialStarsDbContext dbContext)
    {
        var callbackUrlString = await JsonHelper.GetJsonPropertyValueAsync(httpContext, "callback");
        var statusString = await JsonHelper.GetJsonPropertyValueAsync(httpContext, "status");

        var webhook = dbContext.Webhooks.Include(webhook => webhook.LogEntries)
            .FirstOrDefault(webhook => webhook.CallbackUrl.EndsWith(callbackUrlString));

        webhook?.LogEntries.Add(new LogEntry
        {
            Date = DateTime.Now,
            Text = $"Subscription Entfernt | {statusString}"
        });

        await dbContext.SaveChangesAsync();

        httpContext.Response.StatusCode = 200;
        await httpContext.Response.WriteAsync("");
    }

    private static async Task ProcessWebhookAction(Webhook webhook) { }
}