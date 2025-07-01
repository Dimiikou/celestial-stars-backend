namespace CelestialStars_Domain.exceptions;

public class WebhookNotFoundException : Exception
{
    public WebhookNotFoundException(string type)
    : base($"No Webhook found with type {type}")
    {

    }
}