namespace UserManagementService.Models.Base;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedTimestamp { get; set; }
    public string? DeleteNotes { get; set; }
}
