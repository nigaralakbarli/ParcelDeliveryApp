using Microsoft.AspNetCore.Identity;
using UserManagementService.Models.Base;

namespace UserManagementService.Models;

public class User : IdentityUser, ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedTimestamp { get; set; }
    public string? DeleteNotes { get; set; }
}
