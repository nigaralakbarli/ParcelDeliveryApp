namespace Shared.Dtos.Order;

public record OrderUpdateDto(
    int Id,
    int UserId,
    string AddressLine,
    double OrderTotal);