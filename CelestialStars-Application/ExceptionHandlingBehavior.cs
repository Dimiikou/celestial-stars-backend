using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using CelestialStars_Domain.exceptions;
using FluentValidation;
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
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            throw new ApiException(JsonSerializer.Serialize(errors), HttpStatusCode.BadRequest.GetHashCode(), ex);
        }
        catch (UserAlreadyExistingException ex)
        {
            throw new ApiException("User already exists", HttpStatusCode.BadRequest.GetHashCode(), ex);
        }
        catch (InvalidCredentialException ex)
        {
            throw new ApiException("Invalid credentials", HttpStatusCode.BadRequest.GetHashCode(), ex);
        }
        catch (EventSubMessageTypeNotFoundException ex)
        {
            throw new ApiException("EventSubMessageType in Header not found or not correct", HttpStatusCode.BadRequest.GetHashCode(), ex);
        }
        catch (JsonException ex)
        {
            throw new ApiException("Error while deserializing Json", HttpStatusCode.BadRequest.GetHashCode(), ex);
        }
        catch (Exception ex)
        {
            // Unerwarteter Fehler
            throw new ApiException("Internal server error", HttpStatusCode.InternalServerError.GetHashCode(), ex);
        }
    }
}