using Shared.Dtos.Order;
using Shared.Enums;

namespace OrderMicroservice.Services.Abstraction;

public interface IOrdersService
{
    Task CreateOrderAsync(OrderCreateDTO orderCreateDTO);
    Task<bool> UpdateOrderAsync(OrderUpdateDto orderUpdateDTO);
    Task<bool> DeleteOrderAsync(int oderId);
    Task<bool> CancleOrderAsync(int orderId);
    Task<bool> ChangeOrderStatusAsync(int orderId, OrderStatus status); 
    Task<List<OrderResponseDto>> GetOrdersAsync();
    Task<OrderResponseDto> GetOrderById(int orderId);
    Task<List<OrderResponseDto>> GetUserOrders();
    Task<bool> ChangeOrderDestination(int orderId, string destination);
}
