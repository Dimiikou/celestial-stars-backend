using CelestialStars_Application.webhooks.twitch.Validators;
using FluentValidation;

namespace CelestialStars_Application.webhooks.twitch.revocation;

public class TwitchRevocationRequestValidator : AbstractValidator<TwitchRevocationRequest>
{
    public TwitchRevocationRequestValidator()
    {
        RuleFor(x => x.Subscription).NotNull().SetValidator(new SubscriptionValidator());
    }
}