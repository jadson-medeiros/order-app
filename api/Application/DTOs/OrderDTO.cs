using Domain.Entities;

namespace Application.DTOs;

public class OrderDTO : Entity
{
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsPaid { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
}