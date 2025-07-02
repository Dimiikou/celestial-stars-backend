using CelestialStars_Domain.exceptions;
using CelestialStars_Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Application.webhooks.twitch.eventFired;

public class TwitchEventFiredHandler : IRequestHandler<TwitchEventFiredRequest, Unit>
{
    private readonly CelestialStarsDbContext _db;

    public TwitchEventFiredHandler(CelestialStarsDbContext db)
    {
        _db = db;
    }

    public async Task<Unit> Handle(TwitchEventFiredRequest request, CancellationToken cancellationToken)
    {
        // TODO: Implement Logic to Handle Webhook Action
        var subscriptionType = request.Subscription.Type;
        var webhook = await _db.Webhooks.FirstOrDefaultAsync(webhook => webhook.SubscribedEvent.Equals(subscriptionType), cancellationToken);

        if (webhook is null)
        {
            throw new WebhookNotFoundException(subscriptionType);
        }

        return Unit.Value;
    }
}