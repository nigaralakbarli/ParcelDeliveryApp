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
    private readonly IOrderStatusChangeRepository _orderStatusChangeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IKafkaService _kafkaService;


    public OrdersService(
        IOrderRepository orderRepository,
        IOrderStatusChangeRepository orderStatusChangeRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IKafkaService kafkaService)
    {
        _orderRepository = orderRepository;
        _orderStatusChangeRepository = orderStatusChangeRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _kafkaService = kafkaService;
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
        if(order == null && order.OrderStatus == OrderStatus.Delivered)
        {
            return false;
        }

        order.OrderStatus = OrderStatus.Cancelled;
        await _orderRepository.UpdateAsync(order);
        _kafkaService.PublishMessage("order-canceled", order.Id.ToString(), JsonConvert.SerializeObject(order));

        var statusChange = new OrderStatusChange
        {
            OrderId = order.Id,
            NewStatus = OrderStatus.Cancelled,
            ChangeDateTime = DateTime.UtcNow
        };

        await _orderStatusChangeRepository.AddAsync(statusChange);
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

        _kafkaService.PublishMessage("order-destination", order.Id.ToString(), JsonConvert.SerializeObject(order));

        return true;    
    }

    public async Task CreateOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        var mapped = _mapper.Map<Order>(orderCreateDTO);
        mapped.UserId = await GetCurrentUserIdAsync();
        await _orderRepository.AddAsync(mapped);

        var initialStatusChange = new OrderStatusChange
        {
            OrderId = mapped.Id,
            NewStatus = OrderStatus.Pending,
            ChangeDateTime = DateTime.UtcNow
        };
        await _orderStatusChangeRepository.AddAsync(initialStatusChange);

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
            await _orderRepository.UpdateAsync(order);


            _kafkaService.PublishMessage("order-status", order.Id.ToString(), JsonConvert.SerializeObject(order));

            var statusChange = new OrderStatusChange
            {
                OrderId = order.Id,
                NewStatus = status,
                ChangeDateTime = DateTime.UtcNow 
            };

            await _orderStatusChangeRepository.AddAsync(statusChange);
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
        await ChangeOrderStatusAsync(order.Id, order.OrderStatus);
    }

    public async void OrderAssignedEventHandler(string message)
    {
        var order = JsonConvert.DeserializeObject<Order>(message);
        await _orderRepository.UpdateAsync(order);
    }

}
