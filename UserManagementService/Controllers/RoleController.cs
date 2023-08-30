using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Dtos.Role;
using UserManagementService.Services.Abstraction;

namespace UserManagementService.Controllers
{
    [Route("Role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            return Ok(await _roleService.GetAllRolesAsync());
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetRoleAsync(string id)
        {
            var result = await _roleService.GetRoleAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest($"Role with ID {id} not found");
        }

        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetRolesByUser(string userName)
        {
            var result = await _roleService.GetRoleByUser(userName);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest($"Username not found");
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoleAsync(RoleDto roleDto)
        {
            if (await _roleService.RoleExistsAsync(roleDto.Id))
            {
                return BadRequest("Role already exist");
            }
            if (!await _roleService.CreateRoleAsync(roleDto))
            {
                return BadRequest("Something went wrong");
            }
            return Ok("Role successful created");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoleAsync(string id)
        {
            if (await _roleService.DeleteRoleAsync(id))
            {
                return Ok("Successfully deleted");
            }
            return NotFound();
        }

        [HttpPut("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUserAsync(UserRoleDto userRoleDto)
        {
            if (!await _roleService.AddRoleToUserAsync(userRoleDto))
            {
                return BadRequest();
            }
            return Ok("Roles added to user");
        }

        [HttpPut("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUserAsync(UserRoleDto userRoleDto)
        {
            if (!await _roleService.RemoveRoleFromUserAsync(userRoleDto))
            {
                return BadRequest();
            }
            return Ok("Roles removed from user");
        }
    }
}
