using AutoMapper;
using Newtonsoft.Json;
using OrderMicroservice.Repositories.Abstraction;
using OrderMicroservice.Services.Abstraction;
using Shared.Dtos.Order;
using Shared.Enums;
using Shared.Models;
using Shared.Services.Abstraction;
using Shared.Services.Concrete;
using System.Security.Claims;

namespace OrderMicroservice.Services.Concrete;

public class OrdersService : IOrdersService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IKafkaService _kafkaService;


    public OrdersService(
        IOrderRepository orderRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IKafkaService kafkaService)
    {
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _kafkaService = kafkaService;
    }
    
    public async Task<OrderResponseDto> GetOrderById(int orderId)
    {
        var order = await _orderRepository.GetByIdIncludeAsync(orderId);
        return _mapper.Map<OrderResponseDto>(order);
    }

    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetAllIncludeAsync();
        return _mapper.Map<List<OrderResponseDto>>(orders);
    }

    public async Task<bool> CancleOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if(order == null && order.OrderStatus == OrderStatus.Delivered)
        {
            return false;
        }

        order.OrderStatus = OrderStatus.Cancelled;

        var statusChange = new OrderStatusChange
        {
            NewStatus = OrderStatus.Cancelled,
            ChangeDateTime = DateTime.UtcNow
        };

        order.StatusChanges.Add(statusChange);
        await _orderRepository.UpdateAsync(order);

        _kafkaService.PublishMessage("order-updated", order.Id.ToString(), JsonConvert.SerializeObject(order));

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

        _kafkaService.PublishMessage("order-updated", order.Id.ToString(), JsonConvert.SerializeObject(order));

        return true;    
    }

    public async Task CreateOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        var mapped = _mapper.Map<Order>(orderCreateDTO);
        var initialStatusChange = new OrderStatusChange
        {
            NewStatus = OrderStatus.Pending,
            ChangeDateTime = DateTime.UtcNow
        };
        mapped.StatusChanges = new List<OrderStatusChange> { initialStatusChange };
        mapped.UserId = await GetCurrentUserIdAsync();
        await _orderRepository.AddAsync(mapped);

        _kafkaService.PublishMessage("order-created", mapped.Id.ToString(), JsonConvert.SerializeObject(mapped));
    }

    public async Task<bool> DeleteOrderAsync(int oderId)
    {
        var order = await  _orderRepository.GetByIdAsync(oderId);
        if (order != null)
        {
            await _orderRepository.RemoveAsync(order);

            _kafkaService.PublishMessage("order-deleted", order.Id.ToString(), JsonConvert.SerializeObject(order));

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

            _kafkaService.PublishMessage("order-updated", order.Id.ToString(), JsonConvert.SerializeObject(order));

            return true;
        }
        return false;
    }

    public async Task<bool> ChangeOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if(order != null)
        {
            order.OrderStatus = status;
            var statusChange = new OrderStatusChange
            {
                NewStatus = status,
                ChangeDateTime = DateTime.UtcNow 
            };

            order.StatusChanges ??= new List<OrderStatusChange>();
            order.StatusChanges.Add(statusChange);
            await _orderRepository.UpdateAsync(order);

            _kafkaService.PublishMessage("order-updated", order.Id.ToString(), JsonConvert.SerializeObject(order));

            return true;
        }
        return false;
    }

    private async Task<string> GetCurrentUserIdAsync()
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async void OrderDeliveredEventHandler(string message)
    {
        var order = JsonConvert.DeserializeObject<Order>(message);
        await _orderRepository.AddAsync(order);
    }   

}
