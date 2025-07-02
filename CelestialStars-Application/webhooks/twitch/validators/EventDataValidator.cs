using CelestialStars_Domain.TwitchWebhook;
using FluentValidation;

namespace CelestialStars_Application.webhooks.twitch.Validators;

public class EventDataValidator : AbstractValidator<EventData>
{
    public EventDataValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.UserLogin).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();

        RuleFor(x => x.BroadcasterUserId).NotEmpty();
        RuleFor(x => x.BroadcasterUserLogin).NotEmpty();
        RuleFor(x => x.BroadcasterUserName).NotEmpty();

        RuleFor(x => x.FollowedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("FollowedAt cannot be in the future.");
    }
}