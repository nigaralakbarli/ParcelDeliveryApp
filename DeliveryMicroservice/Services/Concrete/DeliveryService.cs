using AutoMapper;
using DeliveryMicroservice.Repositories.Abstraction;
using DeliveryMicroservice.Services.Abstraction;
using Shared.Dtos.Delivery;
using Shared.Dtos.Order;
using Shared.Enums;
using Shared.Models;

namespace DeliveryMicroservice.Services.Concrete;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public DeliveryService(
        IDeliveryRepository deliveryRepository,
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _deliveryRepository = deliveryRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    public async Task<bool> AssignOrderAsync(int orderId, string courierId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return false;
        }

        var delivery = new Delivery
        {
            OrderId = orderId,
            CourierId = courierId,
            DeliveryStatus = DeliveryStatus.PendingPickup 
        };

        var initialStatusChange = new DeliveryStatusChange
        {
            NewStatus = DeliveryStatus.PendingPickup,
            ChangeDateTime = DateTime.UtcNow
        };

        delivery.StatusChanges = new List<DeliveryStatusChange> { initialStatusChange };
    
        await _deliveryRepository.AddAsync(delivery);
        order.CourierId = courierId;
        await _orderRepository.UpdateAsync(order);
        return true;
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
        return true;

    }

    public async Task<List<OrderResponseDto>> GetCourierOrders(string courierId)
    {
        var deliveries = await _orderRepository.FindAsync(o => o.CourierId == courierId);
        return _mapper.Map<List<OrderResponseDto>>(deliveries);
    }

    public async Task<DeliveryResponseDto> GetDeliveryDetails(int deliveryId)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);
        return _mapper.Map<DeliveryResponseDto>(delivery);  
    }

    public async Task<bool> SetDelivered(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return false;
        }

        var delivery = (await _deliveryRepository.FindAsync(d => d.OrderId == orderId)).FirstOrDefault();

        if (delivery == null)
        {
            return false;
        }

        delivery.DeliveryStatus = DeliveryStatus.DeliveredSuccessfully;
        var statusChange = new DeliveryStatusChange
        {
            NewStatus = DeliveryStatus.DeliveredSuccessfully,
            ChangeDateTime = DateTime.UtcNow
        };

        delivery.StatusChanges ??= new List<DeliveryStatusChange>();
        delivery.StatusChanges.Add(statusChange);
        await _deliveryRepository.UpdateAsync(delivery);


        //Kafka
        order.OrderStatus = OrderStatus.Delivered;
        await _orderRepository.UpdateAsync(order);
        return true;
    }
}
