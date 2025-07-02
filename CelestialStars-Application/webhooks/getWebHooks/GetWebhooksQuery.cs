using CelestialStars_Domain;
using MediatR;

namespace CelestialStars_Application.webhooks.getWebHooks;

public class GetWebhooksQuery : IRequest<List<Webhook>>
{
    
}