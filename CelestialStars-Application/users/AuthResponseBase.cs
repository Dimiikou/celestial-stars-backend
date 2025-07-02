using CelestialStars_Domain.entity;

namespace CelestialStars_Application.Users;

public class AuthResponseBase
{
    public required UserDto User { get; set; }
    public required string Token { get; set; }
}