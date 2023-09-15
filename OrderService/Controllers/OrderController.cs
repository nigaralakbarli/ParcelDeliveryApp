using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.Services.Abstraction;
using Shared.Dtos.Order;

namespace OrderMicroservice.Controllers
{
    [Route("Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrderController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrdersAsync()
        {
            return Ok(await _ordersService.GetOrdersAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _ordersService.GetOrderById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"Order with ID {id} not found");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOrderAsync(OrderUpdateDto orderUpdateDto)
        {
            if (await _ordersService.UpdateOrderAsync(orderUpdateDto))
            {
                return Ok("Successfully updated");
            }
            return NotFound();
        }

        [HttpPut("ChangeDestination")]
        public async Task<IActionResult> ChangeOrderDestination(int orderId, string destination)
        {
            if (await _ordersService.ChangeOrderDestination(orderId, destination))
            {
                return Ok("Successfully updated");
            }
            return BadRequest();
        }

        [HttpPut("Cancle")]
        public async Task<IActionResult> CancleOrderAsync(int orderId)
        {
            if (await _ordersService.CancleOrderAsync(orderId))
            {
                return Ok("Successfully updated");
            }
            return NotFound();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            if (await _ordersService.DeleteOrderAsync(id))
            {
                return Ok("Successfully deleted");
            }
            return NotFound();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            await _ordersService.CreateOrderAsync(orderCreateDTO);
            return Ok(orderCreateDTO);
        }

        [HttpGet("UserOrders")]
        public async Task<ActionResult<List<OrderResponseDto>>> GetUserOrders()
        {
            return Ok(await _ordersService.GetUserOrders());
        }
    }
}
