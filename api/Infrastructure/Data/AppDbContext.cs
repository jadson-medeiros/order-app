using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ProductName = "Smartphone XYZ", Price = 1200.0m },
            new Product { Id = 2, ProductName = "Notebook ABC", Price = 2500.0m },
            new Product { Id = 3, ProductName = "Bluetooth Headphones", Price = 150.0m },
            new Product { Id = 4, ProductName = "RGB Mechanical Keyboard", Price = 180.0m },
            new Product { Id = 5, ProductName = "Gamer Mouse XYZ", Price = 80.0m }
        );
    }
}