using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Dtos.User;
using UserManagementService.Services.Abstraction;

namespace UserManagementService.Controllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(
            IAuthService authService,
            IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginDto loginDto)
        {
            var tokenResponse = await _authService.LoginAsync(loginDto);
            if (tokenResponse != null)
            {
                return Ok(tokenResponse);
            }
            return NotFound("User not found");
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsersAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"User with ID {id} not found");
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(RegistrationDto registrationDto)
        {
            if (await _userService.UserExistsAsync(registrationDto.Email))
            {
                return BadRequest("User already exist");
            }

            return Ok(await _userService.RegisterAsync(registrationDto));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser(UserRequestDto userRequestDto)
        {
            if (await _userService.UpdateUserAsync(userRequestDto))
            {
                return Ok("Successfully updated");
            }
            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (await _userService.DeleteUserAsync(id))
            {
                return Ok("Successfully deleted");
            }
            return NotFound();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (await _userService.ChangePasswordAsync(changePasswordDto))
            {
                return Ok("Password changed successfully");
            }
            return NotFound();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string userName, string NewPassword)
        {
            if (await _userService.ResetPasswordAsync(userName, NewPassword))
            {
                return Ok("Password reset successfully");
            }
            return NotFound();
        }
    }
}
