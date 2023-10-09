using Shared.Dtos.Delivery;
using Shared.Dtos.Order;
using Shared.Enums;
using Shared.Models;

namespace DeliveryMicroservice.Services.Abstraction;

public interface IDeliveryService
{
    Task<List<DeliveryResponseDto>> GetDeliveriesAsync();  
    Task<bool> AssignOrderAsync(int orderId, string courierId);
    Task<bool> ChangeDeliveryStatus(int deliveryId, DeliveryStatus status); 
    Task<List<OrderResponseDto>> GetCourierOrders(string courierId);    
    Task<DeliveryResponseDto> GetDeliveryDetails(int deliveryId);
    Task<List<Order>> GetOrdersTest();
    void OrderCreatedEventHandler(string message);
    void OrderUpdateEventHandler(string message);
    void OrderDeleteEventHandler(string message);
}
