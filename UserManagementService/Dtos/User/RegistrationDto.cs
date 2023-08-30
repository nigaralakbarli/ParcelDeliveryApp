using System.Reflection.Metadata;

namespace UserManagementService.Dtos.User;

public record RegistrationDto(
    string UserName,
    string Password,
    string Email);
