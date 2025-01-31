using CelestialStars_Api.services;
using CelestialStars_Domain;
using CelestialStars_Domain.dataTransferObjects;
using CelestialStars_Sql;
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

    private static async Task<IResult> Register(HttpContext context, RegisterDto registerDto, AuthService authService, CelestialStarsDbContext db)
    {
        if (await db.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            return Results.BadRequest("Email already in use");
        }

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = authService.HashPassword(registerDto.Password)
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var token = authService.CreateToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(1)
        };

        context.Response.Cookies.Append("jwt", token, cookieOptions);

        return Results.Ok(new
        {
            user = new { user.Id, user.Username, user.Email },
            message = "Registration successful"
        });
    }

    private static IResult Logout(HttpContext context)
    {
        context.Response.Cookies.Delete("jwt");
        return Results.Ok(new { message = "Logged out successfully" });
    }

    private static async Task<IResult> Login(HttpContext context, LoginDto loginDto, AuthService authService, CelestialStarsDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            return Results.BadRequest("Username or Password Incorrect");
        }

        if (!authService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return Results.BadRequest("Username or Password Incorrect");
        }

        var token = authService.CreateToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(1)
        };

        context.Response.Cookies.Append("jwt", token, cookieOptions);

        return Results.Ok(new
        {
            user = new { user.Id, user.Username, user.Email },
            message = "Login successful"
        });
    }
}