using CelestialStars_Domain;
using CelestialStars_Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Application.webhooks.getWebHooks;

public class GetWebhooksHandler : IRequestHandler<GetWebhooksQuery, List<Webhook>>
{
    private readonly CelestialStarsDbContext _db;

    public GetWebhooksHandler(CelestialStarsDbContext db)
    {
        _db = db;
    }

    public async Task<List<Webhook>> Handle(GetWebhooksQuery request, CancellationToken cancellationToken)
    {
        return await _db.Webhooks.ToListAsync(cancellationToken);
    }
}