using Shared.Enums;

namespace Shared.Dtos.Order;

public record OrderStatusChangeDto(
    string NewStatus,
    DateTime ChangeDateTime);

