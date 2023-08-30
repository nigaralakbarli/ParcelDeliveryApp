using AutoMapper;
using UserManagementService.Dtos.Role;
using UserManagementService.Dtos.User;
using UserManagementService.Models;

namespace UserManagementService.Mapper;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<User, UserResponseDto>();
        CreateMap<UserRequestDto, User>();
        CreateMap<RegistrationDto, User>();

        CreateMap<RoleDto, Role>().ReverseMap();

    }
}
