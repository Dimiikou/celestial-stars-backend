namespace CelestialStars_Domain;

public class LogEntry
{
    public int Id { get; set; }

    public int WebhookId { get; set; } // Foreign key
    public Webhook Webhook { get; set; } // Navigation property

    public DateTime Date { get; set; }

    public string Text { get; set; }
}