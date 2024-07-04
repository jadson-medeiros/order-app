using Domain.Entities;
using Domain.Repositories;
using Moq;

namespace Domain.Tests.Repositories;

public class OrderRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnOrders()
    {
        // Arrange
        var mockRepository = new Mock<IOrderRepository>();
        var orders = new List<Order>
        {
            new Order { Id = 1, CustomerName = "Customer 1", IsPaid = true },
            new Order { Id = 2, CustomerName = "Customer 2", IsPaid = false }
        };
        mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);
        var repository = mockRepository.Object;

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}