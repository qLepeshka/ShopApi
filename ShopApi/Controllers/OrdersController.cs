using Microsoft.AspNetCore.Mvc;
using ShopApi.Models;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_orderService.GetAllOrders());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            var createdOrder = _orderService.CreateOrder(order);
            return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Order order)
        {
            try
            {
                var updatedOrder = _orderService.UpdateOrder(id, order);
                return Ok(updatedOrder);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("status/{status}")]
        public IActionResult GetByStatus(string status)
        {
            return Ok(_orderService.GetOrdersByStatus(status));
        }

        [HttpPost("{orderId}/products/{productId}")]
        public IActionResult AddProduct(int orderId, int productId, [FromQuery] int quantity = 1)
        {
            try
            {
                _orderService.AddProductToOrder(orderId, productId, quantity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{orderId}/products/{productId}")]
        public IActionResult RemoveProduct(int orderId, int productId)
        {
            try
            {
                _orderService.RemoveProductFromOrder(orderId, productId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
