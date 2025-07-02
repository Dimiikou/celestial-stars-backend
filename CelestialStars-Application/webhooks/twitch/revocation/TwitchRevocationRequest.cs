using CelestialStars_Domain.TwitchWebhook;
using MediatR;

namespace CelestialStars_Application.webhooks.twitch.revocation;

public class TwitchRevocationRequest : TwitchSubscriptionEnvelope, IRequest<Unit> { }