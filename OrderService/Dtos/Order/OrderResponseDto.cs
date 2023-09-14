using OrderMicroservice.Enums;

namespace OrderMicroservice.Dtos.Order;

public record OrderResponseDto(
    int Id,
    string UserId,
    string AddressLine,
    DateTime OrderDate,
    double OrderTotal,
    string OrderStatus);
