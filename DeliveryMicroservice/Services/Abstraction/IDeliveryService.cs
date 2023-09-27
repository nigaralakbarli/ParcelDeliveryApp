namespace DeliveryMicroservice.Services.Abstraction;

public interface IDeliveryService
{
    Task<bool> AssignOrderAsync(int orderId, string CourierId);
    Task<bool> SetDelivered(int orderId);
}
