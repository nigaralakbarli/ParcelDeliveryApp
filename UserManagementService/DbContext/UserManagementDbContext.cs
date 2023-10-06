using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementService.Models;

namespace UserManagementService.DbContext;

public class UserManagementDbContext : IdentityDbContext<User, Role, string>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=usermanagementdb; Port= 5432;Database=usermanagement_db;User Id=postgres;Password=mypassword;");
    }

}
