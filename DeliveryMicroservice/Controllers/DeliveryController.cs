using DeliveryMicroservice.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

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
}
