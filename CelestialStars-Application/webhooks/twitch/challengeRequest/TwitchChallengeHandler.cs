using MediatR;
using Microsoft.AspNetCore.Http;

namespace CelestialStars_Application.webhooks.twitch.challengeRequest;

public class TwitchChallengeHandler : IRequestHandler<TwitchChallengeRequest, Unit>
{
    private readonly HttpContext _httpContext;

    public TwitchChallengeHandler(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public async Task<Unit> Handle(TwitchChallengeRequest request, CancellationToken cancellationToken)
    {
        _httpContext.Response.StatusCode = 200;
        await _httpContext.Response.WriteAsync(request.Challenge, cancellationToken: cancellationToken);
        return Unit.Value;
    }
}