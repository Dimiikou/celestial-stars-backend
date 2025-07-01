using FluentValidation;

namespace CelestialStars_Application.Users.login;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}