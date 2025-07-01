using System.Security.Authentication;
using CelestialStars_Domain.exceptions;
using MediatR;

namespace CelestialStars_Application;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (UserAlreadyExistingException ex)
        {
            throw new ApiException("User already exists", 400, ex);
        }
        catch (InvalidCredentialException ex)
        {
            throw new ApiException("Invalid credentials", 400, ex);
        }
        catch (Exception ex)
        {
            // Unerwarteter Fehler
            throw new ApiException("Internal server error", 500, ex);
        }
    }
}