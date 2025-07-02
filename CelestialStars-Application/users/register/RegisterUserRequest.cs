using CelestialStars_Application.Users;
using MediatR;

namespace CelestialStars_Application.users.register;

public class RegisterUserRequest : IRequest<RegisterUserResponse>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}