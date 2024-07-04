using Domain.Entities;

namespace Application.DTOs;

public class OrderItemDTO : Entity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}