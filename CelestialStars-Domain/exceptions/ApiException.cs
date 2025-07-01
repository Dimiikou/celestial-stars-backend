namespace CelestialStars_Domain.exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }

    public ApiException(string message, int statusCode = 500, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}