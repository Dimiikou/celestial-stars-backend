namespace CelestialStars_Domain.exceptions;

public class EventSubMessageTypeNotFoundException : Exception
{
    public EventSubMessageTypeNotFoundException()
        : base("EventSubMessageType in Header not found or not correct") { }
}