using MediatR;
using Microsoft.AspNetCore.Http;

namespace CelestialStars_Application.webhooks.twitch.challengeRequest;

public class TwitchChallengeHandler : IRequestHandler<TwitchChallengeRequest, string>
{
    public TwitchChallengeHandler() { }

    public Task<string> Handle(TwitchChallengeRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Challenge);
    }
}