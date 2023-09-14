using DeliveryMicroservice.Enums;

namespace DeliveryMicroservice.Models;

public class Delivery : EntityBase
{
    public string UserId { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public DateTime OrderDate { get; set; } 
    public double OrderTotal { get; set; }  
    public string CourierId { get; set; } = default!;
    public OrderStatus OrderStatus { get; set; }
}
