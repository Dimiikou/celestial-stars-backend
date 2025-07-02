namespace CelestialStars_Domain.exceptions;

public class WebhookNotFoundException : Exception
{
    public WebhookNotFoundException(string type)
    : base($"No Webhook found with type {type}")
    {
    }

    public WebhookNotFoundException(int id)
        : base($"No Webhook found with Id {id}")
    {
    }
}