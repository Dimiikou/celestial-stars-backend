using CelestialStars_Domain.TwitchWebhook;

namespace CelestialStars_Domain.entity.TwitchWebhook;

public class Subscription
{
    public string Id { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string Version { get; set; }
    public int Cost { get; set; }
    public Condition Condition { get; set; }
    public Transport Transport { get; set; }
    public DateTime CreatedAt { get; set; }
}