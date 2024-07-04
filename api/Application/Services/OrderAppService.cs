using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class OrderAppService : IOrderAppService
{
    private readonly IOrderRepository _orderRepository;

    public OrderAppService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(o => new OrderDTO
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            CustomerEmail = o.CustomerEmail,
            CreationDate = o.CreationDate,
            IsPaid = o.IsPaid,
            OrderItems = o.OrderItems.Select(i => new OrderItemDTO
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        });
    }

    public async Task<OrderDTO> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        return new OrderDTO
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            CreationDate = order.CreationDate,
            IsPaid = order.IsPaid,
            OrderItems = order.OrderItems.Select(i => new OrderItemDTO
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    public async Task AddOrderAsync(OrderDTO orderDTO)
    {
        var order = new Order
        {
            CustomerName = orderDTO.CustomerName,
            CustomerEmail = orderDTO.CustomerEmail,
            CreationDate = orderDTO.CreationDate,
            IsPaid = orderDTO.IsPaid,
            OrderItems = orderDTO.OrderItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };
        await _orderRepository.AddAsync(order);
    }

    public async Task UpdateOrderAsync(OrderDTO orderDTO)
    {
        var order = await _orderRepository.GetByIdAsync(orderDTO.Id);
        if (order == null)
            throw new Exception("Order not found");

        order.CustomerName = orderDTO.CustomerName;
        order.CustomerEmail = orderDTO.CustomerEmail;
        order.CreationDate = orderDTO.CreationDate;
        order.IsPaid = orderDTO.IsPaid;

        UpdateOrderItems(order, orderDTO.OrderItems);

        await _orderRepository.UpdateAsync(order);
    }

    public async Task DeleteOrderAsync(int id)
    {
        await _orderRepository.DeleteAsync(id);
    }
    
    private void UpdateOrderItems(Order order, List<OrderItemDTO> orderItemDTOs)
    {
        var currentOrderItems = order.OrderItems.ToDictionary(i => i.Id);

        foreach (var orderItemDTO in orderItemDTOs)
        {
            if (currentOrderItems.TryGetValue(orderItemDTO.Id, out var currentOrderItem))
            {
                currentOrderItem.ProductId = orderItemDTO.ProductId;
                currentOrderItem.Quantity = orderItemDTO.Quantity;

                currentOrderItems.Remove(orderItemDTO.Id);
            }
            else
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = orderItemDTO.ProductId,
                    Quantity = orderItemDTO.Quantity
                });
            }
        }

        foreach (var remainingOrderItemId in currentOrderItems.Keys)
        {
            var orderItemToRemove = order.OrderItems.FirstOrDefault(i => i.Id == remainingOrderItemId);
            if (orderItemToRemove != null)
                order.OrderItems.Remove(orderItemToRemove);
        }
    }
}