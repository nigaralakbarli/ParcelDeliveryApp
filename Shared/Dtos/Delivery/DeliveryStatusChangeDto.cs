namespace Shared.Dtos.Delivery;

public record DeliveryStatusChangeDto(
    string NewStatus,
    DateTime ChangeDateTime);
