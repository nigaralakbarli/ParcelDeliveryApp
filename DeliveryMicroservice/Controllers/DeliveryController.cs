using DeliveryMicroservice.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Delivery;
using Shared.Dtos.Order;
using Shared.Enums;

namespace DeliveryMicroservice.Controllers;

[Route("Delivery")]
[ApiController]
public class DeliveryController : ControllerBase
{
    private readonly IDeliveryService _deliveryService;

    public DeliveryController(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    [HttpGet("GetCourierOrders")]
    public async Task<ActionResult<List<OrderResponseDto>>> GetCourierOrders(string courierId)
    {
        return Ok(await _deliveryService.GetCourierOrders(courierId));
    }

    [HttpGet("GetDeliveryDetails")]
    public async Task<ActionResult<DeliveryResponseDto>> GetDeliveryDetails(int deliveryId)
    {
        return Ok(await _deliveryService.GetDeliveryDetails(deliveryId));
    }

    [HttpPut("AssignOrder")]
    public async Task<IActionResult> AssignOrder(int orderId, string courierId)
    {
        if (await _deliveryService.AssignOrderAsync(orderId, courierId))
        {
            return Ok("Successfully assigned");
        }
        return NotFound();
    }

    [HttpPut("ChangeDeliveryStatus")]
    public async Task<IActionResult> ChangeDeliveryStatus(int deliveryId, DeliveryStatus status)
    {
        if (await _deliveryService.ChangeDeliveryStatus(deliveryId, status))
        {
            return Ok("Successfully updated");
        }
        return NotFound();
    }

    [HttpGet("OrdersTest")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _deliveryService.GetOrdersTest();
        return Ok(orders);
    }
}
