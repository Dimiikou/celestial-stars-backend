namespace CelestialStars_Domain.exceptions;

public class UserAlreadyExistingException : Exception
{
    public UserAlreadyExistingException(string email)
        : base($"User with Email {email} already exists")
    {
    }
}