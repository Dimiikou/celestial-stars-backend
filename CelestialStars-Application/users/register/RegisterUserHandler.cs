using AutoMapper;
using CelestialStars_Domain.entity;
using CelestialStars_Domain.exceptions;
using CelestialStars_Infrastructure;
using CelestialStars_Infrastructure.services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CelestialStars_Application.users.register;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{

    private readonly CelestialStarsDbContext _db;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;

    public RegisterUserHandler(CelestialStarsDbContext db, AuthService authService, IMapper mapper)
    {
        _db = db;
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var requestEmail = request.Email;
        if (await _db.Users.AnyAsync(user => user.Email == requestEmail, cancellationToken))
        {
            throw new UserAlreadyExistingException(requestEmail);
        }

        var user = _mapper.Map<User>(request);
        user.PasswordHash = _authService.HashPassword(request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        var token = _authService.CreateToken(user);

        return new RegisterUserResponse
        {
            User = _mapper.Map<UserDto>(user),
            Token = token
        };
    }
}