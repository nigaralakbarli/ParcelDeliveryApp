using OrderService.Dtos.Order;

namespace OrderService.Services.Abstraction;

public interface IOrderService
{
    Task<bool> CreateOrderAsync(OrderCreateDTO orderCreateDTO);
    Task<bool> UpdateOrderAsync(OrderUpdateDto orderUpdateDTO);
    Task<bool> DeleteOrderAsync(int oderId);
    Task<bool> CancleOrderAsync(int orderId);
    Task<List<OrderResponseDto>> GetOrdersAsync();
    Task<OrderResponseDto> GetOrderById(int orderId);
    Task<List<OrderResponseDto>> GetUserOrders();
    Task<bool> ChangeOrderDestination(int orderId);
    Task<bool> AssignOrder();
}
