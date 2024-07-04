using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace Application.Tests.Services;

public class OrderAppServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly OrderAppService _orderAppService;

    public OrderAppServiceTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _orderAppService = new OrderAppService(_mockOrderRepository.Object);
    }
    
    [Fact]
    public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { Id = 1, CustomerName = "Customer 1", IsPaid = true },
            new() { Id = 2, CustomerName = "Customer 2", IsPaid = false }
        };
        _mockOrderRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _orderAppService.GetAllOrdersAsync();

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnOrderById()
    {
        // Arrange
        const int orderId = 1;
        var order = new Order
        {
            Id = orderId,
            CustomerName = "Customer 1",
            CustomerEmail = "customer1@example.com",
            CreationDate = DateTime.Now,
            IsPaid = true,
            OrderItems = new List<OrderItem>
            {
                new() { Id = 1, ProductId = 101, Quantity = 2 },
                new() { Id = 2, ProductId = 202, Quantity = 1 }
            }
        };
        _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(order);

        // Act
        var result = await _orderAppService.GetOrderByIdAsync(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        Assert.Equal("Customer 1", result.CustomerName);
        Assert.Equal("customer1@example.com", result.CustomerEmail);
        Assert.Equal(order.CreationDate, result.CreationDate);
        Assert.True(result.IsPaid);
        Assert.NotNull(result.OrderItems);
        Assert.Equal(2, result.OrderItems.Count);
        Assert.Equal(1, result.OrderItems[0].Id);
        Assert.Equal(101, result.OrderItems[0].ProductId);
        Assert.Equal(2, result.OrderItems[0].Quantity);
        Assert.Equal(2, result.OrderItems[1].Id);
        Assert.Equal(202, result.OrderItems[1].ProductId);
        Assert.Equal(1, result.OrderItems[1].Quantity);
    }

    [Fact]
    public async Task AddOrderAsync_ShouldAddNewOrder()
    {
        // Arrange
        var orderDto = new OrderDTO
        {
            CustomerName = "New Customer",
            IsPaid = false,
            OrderItems = new List<OrderItemDTO>
            {
                new() { ProductId = 1, Quantity = 2 },
                new() { ProductId = 2, Quantity = 1 }
            }
        };

        // Act
        await _orderAppService.AddOrderAsync(orderDto);

        // Assert
        _mockOrderRepository.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderAsync_ShouldUpdateExistingOrder()
    {
        // Arrange
        const int orderId = 1;
        var existingOrder = new Order { Id = orderId, CustomerName = "Existing Customer", IsPaid = true };
        _mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId)).ReturnsAsync(existingOrder);

        var orderDto = new OrderDTO
        {
            Id = orderId,
            CustomerName = "Updated Customer",
            IsPaid = false,
            OrderItems = new List<OrderItemDTO>
            {
                new() { ProductId = 1, Quantity = 3 }
            }
        };

        // Act
        await _orderAppService.UpdateOrderAsync(orderDto);

        // Assert
        _mockOrderRepository.Verify(repo => repo.UpdateAsync(existingOrder), Times.Once);
        Assert.Equal("Updated Customer", existingOrder.CustomerName);
        Assert.False(existingOrder.IsPaid);
    }

    [Fact]
    public async Task DeleteOrderAsync_ShouldDeleteOrder()
    {
        // Arrange
        const int orderId = 1;

        // Act
        await _orderAppService.DeleteOrderAsync(orderId);

        // Assert
        _mockOrderRepository.Verify(repo => repo.DeleteAsync(orderId), Times.Once);
    }
}