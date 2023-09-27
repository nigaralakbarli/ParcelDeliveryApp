namespace OrderMicroservice.Models;

public class OrderAssignment
{
    public string OrderId { get; set; }
    public string CourierId { get; set; }
    public DateTime Timestamp { get; set; }
}
