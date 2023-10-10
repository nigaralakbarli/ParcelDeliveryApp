using Shared.Enums;

namespace Shared.Models;

public class Order : EntityBase
{
    public string UserId { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public double OrderTotal { get; set; }
    public string? CourierId { get; set; } = default!;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

}