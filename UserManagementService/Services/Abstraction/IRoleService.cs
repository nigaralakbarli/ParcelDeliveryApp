using UserManagementService.Dtos.Role;

namespace UserManagementService.Services.Abstraction;

public interface IRoleService
{
    Task<bool> CreateRoleAsync(RoleDto roleDto);
    Task<bool> DeleteRoleAsync(string roleId);
    Task<List<RoleDto>> GetAllRolesAsync();
    Task<RoleDto> GetRoleAsync(string roleId);
    Task<bool> AddRoleToUserAsync(UserRoleDto userRoleDto);
    Task<bool> RemoveRoleFromUserAsync(UserRoleDto userRoleDto);    
    Task<bool> RoleExistsAsync(string roleName);
    Task<List<string>> GetRoleByUser(string userName);
}
