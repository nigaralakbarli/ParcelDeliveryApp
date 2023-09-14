using AutoMapper;
using OrderMicroservice.Dtos.Order;
using OrderMicroservice.Repositories.Abstraction;
using OrderMicroservice.Services.Abstraction;
using Shared.Models;
using System.Security.Claims;

namespace OrderMicroservice.Services.Concrete;

public class OrdersService : IOrdersService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public OrdersService(
        IOrderRepository orderRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<OrderResponseDto> GetOrderById(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        return _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return _mapper.Map<List<OrderResponseDto>>(orders);
    }

    public async Task<bool> CancleOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if(order == null && order.OrderStatus == Shared.Enums.OrderStatus.Delivered)
        {
            return false;
        }

        order.OrderStatus = Shared.Enums.OrderStatus.Cancelled;
        await _orderRepository.UpdateAsync(order);
        return true;
        
    }

    public async Task<bool> ChangeOrderDestination(int orderId, string destination)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if( order == null && order.OrderStatus == Shared.Enums.OrderStatus.Delivered)
        {
            return false;
        }

        order.AddressLine = destination;
        await _orderRepository.UpdateAsync(order);
        return true;    
    }

    public async Task CreateOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        var mapped = _mapper.Map<Order>(orderCreateDTO);
        mapped.UserId = await GetCurrentUserIdAsync();
        await _orderRepository.AddAsync(mapped);
    }

    public async Task<bool> DeleteOrderAsync(int oderId)
    {
        var order = await  _orderRepository.GetByIdAsync(oderId);
        if (order != null)
        {
            await _orderRepository.RemoveAsync(order);
            return true;
        }
        return false;
    }

    public async Task<List<OrderResponseDto>> GetUserOrders()
    {
        var userId = await GetCurrentUserIdAsync();
        var orders = await _orderRepository.FindAsync(o => o.UserId == userId);
        return _mapper.Map<List<OrderResponseDto>>(orders);
    }

    public async Task<bool> UpdateOrderAsync(OrderUpdateDto orderUpdateDTO)
    {
        var order = await _orderRepository.GetByIdAsync(orderUpdateDTO.Id);
        if (order != null)
        {
            _mapper.Map(orderUpdateDTO, order);
            await _orderRepository.UpdateAsync(order);
            return true;
        }
        return false;
    }

    private async Task<string> GetCurrentUserIdAsync()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}
