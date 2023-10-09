using AutoMapper;
using DeliveryMicroservice.Repositories.Abstraction;
using DeliveryMicroservice.Services.Abstraction;
using Newtonsoft.Json;
using Shared.Dtos.Delivery;
using Shared.Dtos.Order;
using Shared.Enums;
using Shared.Models;
using Shared.Services.Abstraction;

namespace DeliveryMicroservice.Services.Concrete;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IKafkaService _kafkaService;

    public DeliveryService(
        IDeliveryRepository deliveryRepository,
        IOrderRepository orderRepository,
        IMapper mapper,
        IKafkaService kafkaService)
    {
        _deliveryRepository = deliveryRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
        _kafkaService = kafkaService;
    }


    public async Task<List<DeliveryResponseDto>> GetDeliveriesAsync()
    {
        var delivery = await _deliveryRepository.GetAllIncludeAsync();
        return _mapper.Map<List<DeliveryResponseDto>>(delivery);
    }
    public async Task<bool> AssignOrderAsync(int orderId, string courierId)
    {
        var assigned = await IsCourierAssignedAsync(courierId);
        if (!assigned)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.OrderStatus == OrderStatus.Cancelled)
            {
                return false;
            }

            var delivery = new Delivery
            {
                OrderId = orderId,
                CourierId = courierId,
                DeliveryStatus = DeliveryStatus.PendingPickup,
                Coordinates = new Coordinates
                {
                    Latitude = 0.0,
                    Longitude = 0.0
                }

            };

            var initialStatusChange = new DeliveryStatusChange
            {
                NewStatus = DeliveryStatus.PendingPickup,
                ChangeDateTime = DateTime.UtcNow
            };

            delivery.StatusChanges = new List<DeliveryStatusChange> { initialStatusChange };

            await _deliveryRepository.AddAsync(delivery);
            order.CourierId = courierId;
            _kafkaService.PublishMessage("order-assigned", order.Id.ToString(), JsonConvert.SerializeObject(order));
            await _orderRepository.UpdateAsync(order);
            return true;
        }  
        return false;
    }

    public async Task<bool> ChangeDeliveryStatus(int deliveryId, DeliveryStatus newStatus)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);
        if (delivery == null)
        {
            return false;
        }

        delivery.DeliveryStatus = newStatus;
        var statusChange = new DeliveryStatusChange
        {
            NewStatus = newStatus,
            ChangeDateTime = DateTime.UtcNow
        };

        delivery.StatusChanges ??= new List<DeliveryStatusChange>();
        delivery.StatusChanges.Add(statusChange);
        await _deliveryRepository.UpdateAsync(delivery);

        if (newStatus == DeliveryStatus.DeliveredSuccessfully)
        {
            var order = await _orderRepository.GetByIdAsync(delivery.OrderId);
            if (order == null)
            {
                return false;
            }

            order.OrderStatus = OrderStatus.Delivered;

            _kafkaService.PublishMessage("order-status", order.Id.ToString(), JsonConvert.SerializeObject(order));

            await _orderRepository.UpdateAsync(order);
        }

        return true;

    }

    public async Task<List<OrderResponseDto>> GetCourierOrders(string courierId)
    {
        var deliveries = await _orderRepository.FindAsync(o => o.CourierId == courierId);
        return _mapper.Map<List<OrderResponseDto>>(deliveries);
    }

    public async Task<DeliveryResponseDto> GetDeliveryDetails(int deliveryId)
    {
        var delivery = await _deliveryRepository.GetByIdIncludeAsync(deliveryId);
        return _mapper.Map<DeliveryResponseDto>(delivery);
    }

    public async Task<List<Order>> GetOrdersTest()
    {
        return (await _orderRepository.GetAllAsync()).ToList();
    }

    private async Task<bool> IsCourierAssignedAsync(string courierId)
    {
        var assignedOrders = await _orderRepository.FindAsync(o => o.CourierId == courierId);

        if (assignedOrders == null || !assignedOrders.Any())
        {
            return false;
        }

        foreach (var order in assignedOrders)
        {
            if (order.OrderStatus != OrderStatus.Delivered)
            {
                return true; 
            }
        }

        return false; 
    }


    //Handlers
    public async void OrderCreatedEventHandler(string message)
    {
        var order = JsonConvert.DeserializeObject<Order>(message);
        await _orderRepository.AddAsync(order);
    }

    public async void OrderUpdateEventHandler(string message)
    {
        var order = JsonConvert.DeserializeObject<Order>(message);
        await _orderRepository.UpdateAsync(order);
    }

    public async void OrderDeleteEventHandler(string message)
    {
        var order = JsonConvert.DeserializeObject<Order>(message);
        await _orderRepository.RemoveAsync(order);
    }

}
