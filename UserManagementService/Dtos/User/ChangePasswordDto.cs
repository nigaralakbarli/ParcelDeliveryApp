namespace UserManagementService.Dtos.User;

public record ChangePasswordDto(
    string UserName,
    string CurrentPassword,
    string NewPassword);
