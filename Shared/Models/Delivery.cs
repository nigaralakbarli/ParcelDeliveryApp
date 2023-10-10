using Shared.Enums;

namespace Shared.Models;

public class Delivery : EntityBase
{
    public int OrderId { get; set; }
    public string CourierId { get; set; } = default!;
    public DeliveryStatus DeliveryStatus { get; set; }
    public Coordinates Coordinates { get; set; } = default!;


}
