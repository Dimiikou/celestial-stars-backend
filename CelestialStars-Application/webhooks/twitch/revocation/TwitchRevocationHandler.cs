using CelestialStars_Domain;
using CelestialStars_Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Application.webhooks.twitch.revocation;

public class TwitchRevocationHandler : IRequestHandler<TwitchRevocationRequest, Unit>
{
    private readonly CelestialStarsDbContext _db;
    private readonly HttpContext _httpContext;

    public TwitchRevocationHandler(CelestialStarsDbContext db, HttpContext httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<Unit> Handle(TwitchRevocationRequest request, CancellationToken cancellationToken)
    {
        var webhook = _db.Webhooks.Include(webhook => webhook.LogEntries)
            .FirstOrDefault(webhook => webhook.CallbackUrl.EndsWith(request.Subscription.Transport.Callback));

        webhook?.LogEntries.Add(new LogEntry
        {
            Date = DateTime.Now,
            Text = $"Subscription Entfernt | {request.Subscription.Status}"
        });

        await _db.SaveChangesAsync(cancellationToken);

        _httpContext.Response.StatusCode = 200;
        await _httpContext.Response.WriteAsync("", cancellationToken: cancellationToken);
        return Unit.Value;
    }
}