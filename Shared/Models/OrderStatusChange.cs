using Shared.Enums;

namespace Shared.Models;

public class OrderStatusChange : EntityBase
{
    public OrderStatus NewStatus { get; set; }
    public DateTime ChangeDateTime { get; set; }
}
