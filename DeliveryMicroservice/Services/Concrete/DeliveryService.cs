using AutoMapper;
using DeliveryMicroservice.Repositories.Abstraction;
using DeliveryMicroservice.Services.Abstraction;
using Newtonsoft.Json;
using Shared.Services.Abstraction;

namespace DeliveryMicroservice.Services.Concrete;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly IKafkaService _kafkaService;

    public DeliveryService(
        IDeliveryRepository deliveryRepository,
        IKafkaService kafkaService)
    {
        _deliveryRepository = deliveryRepository;
        _kafkaService = kafkaService;
    }
    public async Task<bool> AssignOrderAsync(int orderId, string courierId)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(orderId);
        if (delivery == null)
        {
            return false;
        }
        delivery.CourierId = courierId;
        await _deliveryRepository.UpdateAsync(delivery);

        // Publish an order assignment event to a Kafka topic
        var orderAssignment = new
        {
            orderId,
            courierId,
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };

        string messageValue = JsonConvert.SerializeObject(orderAssignment);

        return true;
    }

    public void AssignCourierToOrder(string orderId, string courierId)
    {
        UpdateDeliveryStatusInDatabase(orderId, "Assigned to Courier");

        var assignmentMessage = new
        {
            orderId,
            courierId,
            assignmentTimestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };

        string messageValue = JsonConvert.SerializeObject(assignmentMessage);

    }

    private void UpdateDeliveryStatusInDatabase(string orderId, string newStatus)
    {
        // Implement logic to update the delivery status in your database
        Console.WriteLine($"Delivery status updated: OrderID: {orderId}, New Status: {newStatus}");
    }

    public async Task<bool> SetDelivered(int orderId)
    {
        var delivery = await _deliveryRepository.GetByIdAsync(orderId);
        if (delivery == null)
        {
            return false;
        }
        delivery.OrderStatus = Shared.Enums.OrderStatus.Delivered;
        await _deliveryRepository.UpdateAsync(delivery);
        return true;
    }
}
