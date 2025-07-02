using FluentValidation;

namespace CelestialStars_Application.users.register;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username cannot be null or empty.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(20).WithMessage("Username must be between 3 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be null or empty.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be null or empty.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
    }
}