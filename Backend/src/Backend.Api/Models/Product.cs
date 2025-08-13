namespace Backend.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
    public bool? Deleted { get; set; } = false;
    public Category Category { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
}
