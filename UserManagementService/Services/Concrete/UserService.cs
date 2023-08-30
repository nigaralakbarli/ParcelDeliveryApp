using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementService.Dtos.User;
using UserManagementService.Models;
using UserManagementService.Services.Abstraction;

namespace UserManagementService.Services.Concrete;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var user = await _userManager.FindByNameAsync(changePasswordDto.UserName);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        return result.Succeeded;
    }

    public async Task<bool> CreateUserAsync(UserRequestDto userRequestDto)
    {
        var user = await _userManager.FindByNameAsync(userRequestDto.UserName);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.CreateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false; // User not found
        }

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }

    public async Task<UserResponseDto> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByNameAsync(userId);
        if (user == null)
        {
            return null;
        }
        
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<List<UserResponseDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return _mapper.Map<List<UserResponseDto>>(users);
    }

    public async Task<string> RegisterAsync(RegistrationDto registrationDto)
    {
        var user = _mapper.Map<User>(registrationDto);

        var result = await _userManager.CreateAsync(user, registrationDto.Password);
        if(!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return $"User registration failed. Errors: {string.Join(", ", errors)}";
        }

        return "User registered successfully";
    }

    public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if(user == null)
        {
            return false;
        }

        string passResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, passResetToken, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> UpdateUserAsync(UserRequestDto userRequestDto)
    {
        var user = await _userManager.FindByNameAsync(userRequestDto.UserName);
        if( user == null)
        {
            return false;
        }

        _mapper.Map<User>(userRequestDto);
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;

    }

    public async Task<bool> UserExistsAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user != null;
    }
}
