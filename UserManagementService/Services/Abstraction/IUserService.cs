using UserManagementService.Dtos.User;

namespace UserManagementService.Services.Abstraction;

public interface IUserService
{
    Task<List<UserResponseDto>> GetUsersAsync();
    Task<UserResponseDto> GetUserAsync(string userId);  
    Task<bool> CreateUserAsync(UserRequestDto userRequestDto);
    Task<bool> UpdateUserAsync(UserRequestDto userRequestDto);
    Task<bool> DeleteUserAsync(string username);
    Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<bool> ResetPasswordAsync(string username, string newPassword);
    Task<string> RegisterAsync(RegistrationDto registrationDto);
    Task<bool> UserExistsAsync(string userName);
}
