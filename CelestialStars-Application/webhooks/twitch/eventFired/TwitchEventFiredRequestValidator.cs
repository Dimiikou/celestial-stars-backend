using FluentValidation;

namespace CelestialStars_Application.webhooks.twitch.eventFired;

public class TwitchEventFiredRequestValidator : AbstractValidator<TwitchEventFiredRequest>
{
    public TwitchEventFiredRequestValidator()
    {
        RuleFor(x => x.Subscription).NotNull();
        RuleFor(x => x.Event).NotNull();
    }
}