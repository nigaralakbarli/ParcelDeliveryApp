using Shared.Dtos.Delivery;
using Shared.Dtos.Order;
using Shared.Enums;

namespace DeliveryMicroservice.Services.Abstraction;

public interface IDeliveryService
{
    Task<bool> AssignOrderAsync(int orderId, string courierId);
    Task<bool> SetDelivered(int orderId);
    Task<bool> ChangeDeliveryStatus(int deliveryId, DeliveryStatus status); 
    Task<List<OrderResponseDto>> GetCourierOrders(string courierId);    
    Task<DeliveryResponseDto> GetDeliveryDetails(int deliveryId);  
}
