using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CelestialStars_Domain;
using Isopoh.Cryptography.Argon2;
using Microsoft.IdentityModel.Tokens;

namespace CelestialStars_Infrastructure.services;

public class AuthService
{
    private readonly JwtSettings _jwtSettings;

    public AuthService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string HashPassword(string password)
    {
        return Argon2.Hash(password);
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        return Argon2.Verify(storedHash, password);
    }
}