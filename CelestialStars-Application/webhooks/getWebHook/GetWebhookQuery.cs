using CelestialStars_Domain;
using MediatR;

namespace CelestialStars_Application.webhooks.getWebHook;

public class GetWebhookQuery : IRequest<Webhook>
{
    public required int Id { get; set; }
}