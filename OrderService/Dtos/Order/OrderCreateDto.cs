namespace OrderService.Dtos.Order;

public record OrderCreateDTO(
    int Id,
    int AddressId,
    double TotalAmount);