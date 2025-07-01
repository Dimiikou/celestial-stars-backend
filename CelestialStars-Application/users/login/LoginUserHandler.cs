using System.Security.Authentication;
using AutoMapper;
using CelestialStars_Domain;
using CelestialStars_Infrastructure;
using CelestialStars_Infrastructure.services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Application.Users.login;

public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
{
    private readonly CelestialStarsDbContext _db;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;

    public LoginUserHandler(CelestialStarsDbContext db, AuthService authService, IMapper mapper)
    {
        _db = db;
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null)
        {
            throw new InvalidCredentialException();
        }

        if (!_authService.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialException();
        }

        var token = _authService.CreateToken(user);

        return new LoginUserResponse
        {
            Token = token,
            User = _mapper.Map<UserDto>(user),
        };
    }
}