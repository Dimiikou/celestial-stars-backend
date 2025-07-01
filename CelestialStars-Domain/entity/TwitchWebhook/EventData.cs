namespace CelestialStars_Domain.TwitchWebhook;

public class EventData
{
    public string UserId { get; set; }
    public string UserLogin { get; set; }
    public string UserName { get; set; }
    public string BroadcasterUserId { get; set; }
    public string BroadcasterUserLogin { get; set; }
    public string BroadcasterUserName { get; set; }
    public DateTime FollowedAt { get; set; }
}