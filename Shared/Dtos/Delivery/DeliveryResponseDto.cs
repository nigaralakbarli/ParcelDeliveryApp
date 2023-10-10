using Shared.Dtos.Order;
using Shared.Models;

namespace Shared.Dtos.Delivery;
public record DeliveryResponseDto(
    int Id,
    string UserId,
    string DeliveryStatus,
    Coordinates Coordinates);