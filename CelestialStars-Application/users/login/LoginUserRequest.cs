using MediatR;

namespace CelestialStars_Application.Users.login;

public class LoginUserRequest : IRequest<LoginUserResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}