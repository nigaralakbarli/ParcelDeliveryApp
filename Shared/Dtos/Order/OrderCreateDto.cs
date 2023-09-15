namespace Shared.Dtos.Order;

public record OrderCreateDTO(
    int Id,
    string AddressLine,
    double OrderTotal);
