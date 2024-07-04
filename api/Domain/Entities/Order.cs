namespace Domain.Entities;

public class Order : Entity
{
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsPaid { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}