using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderAppService _orderAppService;

    public OrderController(IOrderAppService orderAppService)
    {
        _orderAppService = orderAppService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
    {
        var orders = await _orderAppService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        var order = await _orderAppService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
    {
        if (orderDTO == null)
            return BadRequest();

        await _orderAppService.AddOrderAsync(orderDTO);
        return CreatedAtAction(nameof(GetOrder), new { id = orderDTO.Id }, orderDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderDTO orderDTO)
    {
        if (id != orderDTO.Id)
            return BadRequest();

        var order = await _orderAppService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();

        await _orderAppService.UpdateOrderAsync(orderDTO);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var order = await _orderAppService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();

        await _orderAppService.DeleteOrderAsync(id);
        return NoContent();
    }
}