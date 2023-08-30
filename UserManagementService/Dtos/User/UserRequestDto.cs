namespace UserManagementService.Dtos.User;

public record UserRequestDto(
    string UserName,
    string PhoneNumber,
    string Password);
