using Application.DTOs;

namespace Application.Interfaces;

public interface IOrderAppService
{
    Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
    Task<OrderDTO> GetOrderByIdAsync(int id);
    Task AddOrderAsync(OrderDTO order);
    Task UpdateOrderAsync(OrderDTO order);
    Task DeleteOrderAsync(int id);
}