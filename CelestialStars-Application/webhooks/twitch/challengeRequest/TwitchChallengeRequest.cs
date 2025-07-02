using MediatR;

namespace CelestialStars_Application.webhooks.twitch.challengeRequest;

public class TwitchChallengeRequest : TwitchSubscriptionEnvelope, IRequest<string>
{
    public string Challenge { get; set; }
}