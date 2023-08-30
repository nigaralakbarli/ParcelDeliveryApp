using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementService.Dtos.User;
using UserManagementService.Models;
using UserManagementService.Options;
using UserManagementService.Services.Abstraction;

namespace UserManagementService.Services.Concrete;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly TokenOption _tokenOption;
    private readonly IConfiguration _config;

    public AuthService(
        UserManager<User> userManager,
        TokenOption tokenOption,
        IConfiguration config)
    {
        _userManager = userManager;
        _tokenOption = tokenOption;
        _config = config;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if(user == null)
        {
            return "User does not exist";
        }

        if(!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return "Incorrect Password";
        }

        return await GenerateToken(user);
    }

    private async Task<string> GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var secutiryKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenOption:Key"]));
        var credentials = new SigningCredentials(secutiryKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["TokenOption:Issuer"],
            _config["TokenOption:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(_tokenOption.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
