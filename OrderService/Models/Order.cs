using OrderService.Enums;

namespace OrderService.Models;

public class Order 
{
    public int Id { get; set; } 
    public int UserId { get; set; }
    public string City { get; set; } = default!;
    public int StreetNumber { get; set; }
    public string AddressLine { get; set; } = default!;
    public string Region { get; set; } = default!;
    public DateTime OrderDate { get; set; }
    public double OrderTotal { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public double TotalAmount { get; set; }
}
