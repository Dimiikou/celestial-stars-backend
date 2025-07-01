using CelestialStars_Domain.TwitchWebhook;
using FluentValidation;

namespace CelestialStars_Application.webhooks.twitch.Validators;

public class SubscriptionValidator : AbstractValidator<Subscription>
{
    public SubscriptionValidator()
    {
        RuleFor(x => x.Id).NotNull();
        RuleFor(x => x.Status).NotNull();
        RuleFor(x => x.Type).NotNull();
        RuleFor(x => x.Version).NotNull();
        RuleFor(x => x.Cost).GreaterThanOrEqualTo(0);

        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .LessThanOrEqualTo(x => DateTime.Now)
            .WithMessage("CreatedAt cannot be in the future.");
    }
}