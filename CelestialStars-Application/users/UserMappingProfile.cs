using AutoMapper;
using CelestialStars_Application.users.register;
using CelestialStars_Domain;

namespace CelestialStars_Application.Users;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<UserDto, User>();
    }
}