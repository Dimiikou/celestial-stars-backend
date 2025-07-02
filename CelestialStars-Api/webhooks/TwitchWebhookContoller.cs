using System.Net;
using System.Text.Json;
using CelestialStars_Application;
using CelestialStars_Application.webhooks.twitch.challengeRequest;
using CelestialStars_Application.webhooks.twitch.eventFired;
using CelestialStars_Application.webhooks.twitch.revocation;
using CelestialStars_Domain.exceptions;
using MediatR;

namespace CelestialStars_Api.webhooks;

public static class TwitchWebhookContoller
{
    public static void MapTwitchWebhookEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/twitch", ProcessWebhook);
    }

    private static async Task ProcessWebhook(HttpContext httpContext, ISender sender)
    {
        if (!httpContext.Request.Headers.TryGetValue("twitch-eventsub-message-type", out var eventSubMessageType))
        {
            httpContext.Response.StatusCode = HttpStatusCode.Forbidden.GetHashCode();
            await httpContext.Response.WriteAsync("Signature Header not valid");
            return;
        }

        switch (eventSubMessageType)
        {
            case "webhook_callback_verification":
                await ProcessTwitchChallenge(httpContext, sender);
                break;
            case "revocation":
                await ProcessTwitchRevocation(httpContext, sender);
                break;
            case "notification":
                await ProcessTwitchNotification(httpContext, sender);
                break;
            default:
                throw new EventSubMessageTypeNotFoundException();
        }
    }

    private static async Task ProcessTwitchChallenge(HttpContext httpContext, ISender mediator)
    {
        var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        var challenge = JsonSerializer.Deserialize<TwitchChallengeRequest>(body);
        var challengeString = await mediator.Send(challenge);

        await httpContext.Response.WriteAsync(challengeString);
    }

    private static async Task ProcessTwitchRevocation(HttpContext httpContext, ISender mediator)
    {
        var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        var revocation = JsonSerializer.Deserialize<TwitchRevocationRequest>(body);
        await mediator.Send(revocation);

        await httpContext.Response.WriteJsonAsync("");
    }

    private static async Task ProcessTwitchNotification(HttpContext httpContext, ISender mediator)
    {
        var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        var notification = JsonSerializer.Deserialize<TwitchEventFiredRequest>(body);
        await mediator.Send(notification);

        await httpContext.Response.WriteJsonAsync("");
    }
}