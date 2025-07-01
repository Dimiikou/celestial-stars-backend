using System.Security.Authentication;
using CelestialStars_Application.Users;
using CelestialStars_Application.Users.login;
using CelestialStars_Application.users.register;
using CelestialStars_Domain.dataTransferObjects;
using CelestialStars_Domain.exceptions;
using CelestialStars_Infrastructure;
using CelestialStars_Infrastructure.services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Api.accounting;

public static class AccountController
{
    public static void MapAccountApi(this IEndpointRouteBuilder routeBuilder)
    {
        var accountGroup = routeBuilder.MapGroup("/account")
            .RequireAuthorization();

        accountGroup.MapPost("/register", Register)
            .AllowAnonymous();

        accountGroup.MapPost("/logout", Logout);
        accountGroup.MapPost("/", Login)
            .AllowAnonymous();
    }

    private static async Task<IResult> Register(HttpContext context, RegisterUserRequest registerRequest, ISender mediator)
    {
        var response = await mediator.Send(registerRequest);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(1)
        };

        context.Response.Cookies.Append("jwt", response.Token, cookieOptions);

        return Results.Ok(new
        {
            user = new { response.User.Id, response.User.Username, response.User.Email },
            message = "Registration successful"
        });
    }

    private static IResult Logout(HttpContext context)
    {
        context.Response.Cookies.Delete("jwt");
        return Results.Ok(new { message = "Logged out successfully" });
    }

    private static async Task<IResult> Login(HttpContext context, LoginUserRequest loginRequest, ISender mediator)
    {
        var response = await mediator.Send(loginRequest);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(1)
        };

        context.Response.Cookies.Append("jwt", response.Token, cookieOptions);

        return Results.Ok(new
        {
            user = new { response.User.Id, response.User.Username, response.User.Email },
            message = "Login successful"
        });
    }
}