using CelestialStars_Domain.types;

namespace CelestialStars_Domain.actions;

public class DiscordMessage : IAction
{
    public int Id { get; set; }

    public int ServerId { get; set; }

    public string Message { get; set; } = string.Empty;

    public DiscordMessageTarget Target { get; set; }

    public int TargetId { get; set; }
}