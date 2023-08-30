using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementService.Dtos.Role;
using UserManagementService.Models;
using UserManagementService.Services.Abstraction;

namespace UserManagementService.Services.Concrete;

public class RoleService : IRoleService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;

    public RoleService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<bool> CreateRoleAsync(RoleDto roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);

        var result = await _roleManager.CreateAsync(role);

        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        if (role == null)
        {
            return false; // Role not found
        }

        var result = await _roleManager.DeleteAsync(role);

        return result.Succeeded;
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return _mapper.Map<List<RoleDto>>(roles);
    }

    public async Task<RoleDto> GetRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        if (role == null)
        {
            return null; 
        }

        var roleDto = _mapper.Map<RoleDto>(role);
        return roleDto;
    }

    public async Task<bool> AddRoleToUserAsync(UserRoleDto userRoleDTO)
    {
        var user = await _userManager.FindByNameAsync(userRoleDTO.UserName);

        if (user == null)
        {
            return false; 
        }

        var rolesToAdd = await _roleManager.Roles.Where(r => userRoleDTO.RoleIds.Contains(r.Id)).ToListAsync();

        foreach (var role in rolesToAdd)
        {
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return false; 
            }
        }

        return true; 
    }

    public async Task<bool> RemoveRoleFromUserAsync(UserRoleDto userRoleDTO)
    {
        var user = await _userManager.FindByNameAsync(userRoleDTO.UserName);

        if (user == null)
        {
            return false; 
        }

        var rolesToRemove = await _roleManager.Roles.Where(r => userRoleDTO.RoleIds.Contains(r.Id)).ToListAsync();

        foreach (var role in rolesToRemove)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return false; 
            }
        }

        return true; 
    }
    
    public async Task<bool> RoleExistsAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        return role != null;
    }

    public async Task<List<string>> GetRoleByUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if(user == null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }
}
