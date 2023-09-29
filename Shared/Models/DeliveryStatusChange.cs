using Shared.Enums;

namespace Shared.Models;

public class DeliveryStatusChange : EntityBase
{
    public DeliveryStatus NewStatus { get; set; }
    public DateTime ChangeDateTime { get; set; }
}
