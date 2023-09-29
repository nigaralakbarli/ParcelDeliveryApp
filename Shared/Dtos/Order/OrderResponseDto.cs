using Shared.Models;

namespace Shared.Dtos.Order;

public record OrderResponseDto(
    int Id,
    string UserId,
    string AddressLine,
    DateTime OrderDate,
    double OrderTotal,
    string OrderStatus,
    List<OrderStatusChangeDto> StatusChanges);
