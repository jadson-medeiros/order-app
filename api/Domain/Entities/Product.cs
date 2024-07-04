namespace Domain.Entities;

public class Product : Entity
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}