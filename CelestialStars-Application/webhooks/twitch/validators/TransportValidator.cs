using CelestialStars_Domain.TwitchWebhook;
using FluentValidation;

namespace CelestialStars_Application.webhooks.twitch.Validators;

public class TransportValidator : AbstractValidator<Transport>
{
    public TransportValidator()
    {
        RuleFor(x => x.Method).NotEmpty();
        RuleFor(x => x.Callback)
            .NotEmpty()
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute) && uri.StartsWith("https://api.aissa.dev/", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Callback must be valid absolut Url leading to aissa.dev.");
    }
}