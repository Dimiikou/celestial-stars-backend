using CelestialStars_Application.webhooks.twitch.Validators;
using FluentValidation;

namespace CelestialStars_Application.webhooks.twitch.challengeRequest;

public class TwitchChallengeRequestValidator : AbstractValidator<TwitchChallengeRequest>
{
    public TwitchChallengeRequestValidator()
    {
        RuleFor(x => x.Challenge)
            .NotNull();

        RuleFor(x => x.Subscription)
            .NotNull()
            .SetValidator(new SubscriptionValidator());
    }
}