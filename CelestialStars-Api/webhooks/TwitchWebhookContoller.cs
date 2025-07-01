using System.Net;
using System.Text.Json;
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

    private static async Task ProcessWebhook(HttpContext httpContext, ISender mediator)
    {
        if (!httpContext.Request.Headers.TryGetValue("twitch-eventsub-message-type", out var eventSubMessageType))
        {
            httpContext.Response.StatusCode = HttpStatusCode.Forbidden.GetHashCode();
            await httpContext.Response.WriteAsync("Signature Header not valid");
            return;
        }

        var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
        switch (eventSubMessageType)
        {
            case "webhook_callback_verification":
                var challenge = JsonSerializer.Deserialize<TwitchChallengeRequest>(body);
                await mediator.Send(challenge);
                break;
            case "revocation":
                var revocation = JsonSerializer.Deserialize<TwitchRevocationRequest>(body);
                await mediator.Send(revocation);
                break;
            case "notification":
                var notification = JsonSerializer.Deserialize<TwitchEventFiredRequest>(body);
                await mediator.Send(notification);
                break;
            default:
                throw new EventSubMessageTypeNotFoundException();
        }
    }
}