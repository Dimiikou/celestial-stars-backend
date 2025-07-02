using CelestialStars_Domain.TwitchWebhook;
using FluentValidation;

namespace CelestialStars_Application.webhooks.twitch.Validators;

public class ConditionValidator : AbstractValidator<Condition>
{
    public ConditionValidator()
    {
        RuleFor(x => x.BroadcasterUserId).NotEmpty();
    }
}