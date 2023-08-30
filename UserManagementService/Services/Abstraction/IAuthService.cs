using UserManagementService.Dtos.User;

namespace UserManagementService.Services.Abstraction;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
}
