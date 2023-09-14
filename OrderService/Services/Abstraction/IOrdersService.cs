using OrderMicroservice.Dtos.Order;

namespace OrderMicroservice.Services.Abstraction;

public interface IOrdersService
{
    Task CreateOrderAsync(OrderCreateDTO orderCreateDTO);
    Task<bool> UpdateOrderAsync(OrderUpdateDto orderUpdateDTO);
    Task<bool> DeleteOrderAsync(int oderId);
    Task<bool> CancleOrderAsync(int orderId);
    Task<List<OrderResponseDto>> GetOrdersAsync();
    Task<OrderResponseDto> GetOrderById(int orderId);
    Task<List<OrderResponseDto>> GetUserOrders();
    Task<bool> ChangeOrderDestination(int orderId, string destination);
}
