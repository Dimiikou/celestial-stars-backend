using CelestialStars_Domain.types;

namespace CelestialStars_Domain;

public class Webhook
{
    public int Id { get; set; }

    public string CallbackUrl { get; set; }

    public string SubscribedEvent { get; set; }

    public string Authority { get; set; }

    public ActionType ActionType { get; set; }

    public string JsonEncodedAction { get; set; } = string.Empty;

    public List<LogEntry> LogEntries { get; set; }
}