using Shared.Enums;

namespace Shared.Models;

public class DeliveryStatusChange : EntityBase
{
    public int DeliveryId { get; set; } 
    public DeliveryStatus NewStatus { get; set; }
    public DateTime ChangeDateTime { get; set; }
}
