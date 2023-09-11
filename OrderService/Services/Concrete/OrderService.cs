using OrderService.Dtos.Order;
using OrderService.Repositories.Abstraction;
using OrderService.Services.Abstraction;

namespace OrderService.Services.Concrete;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> AssignOrder()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CancleOrderAsync(int orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ChangeOrderDestination(int orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteOrderAsync(int oderId)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderResponseDto> GetOrderById(int orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<OrderResponseDto>> GetUserOrders()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateOrderAsync(OrderUpdateDto orderUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
