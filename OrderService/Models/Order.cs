using OrderMicroservice.Enums;

namespace OrderMicroservice.Models;

public class Order : EntityBase
{
    public string UserId { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public double OrderTotal { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
}
