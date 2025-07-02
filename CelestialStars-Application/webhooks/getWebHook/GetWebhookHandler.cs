using CelestialStars_Domain;
using CelestialStars_Domain.exceptions;
using CelestialStars_Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Application.webhooks.getWebHook;

public class GetWebhookHandler : IRequestHandler<GetWebhookQuery, Webhook>
{
    private readonly CelestialStarsDbContext _db;

    public GetWebhookHandler(CelestialStarsDbContext db)
    {
        _db = db;
    }

    public async Task<Webhook> Handle(GetWebhookQuery request, CancellationToken cancellationToken)
    {
        var webhook = await _db.Webhooks.FirstOrDefaultAsync(webhook => webhook.Id == request.Id, cancellationToken);

        if (webhook is null)
        {
            throw new WebhookNotFoundException(request.Id);
        }

        return webhook;
    }
}