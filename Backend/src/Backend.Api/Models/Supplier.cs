namespace Backend.Api.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ContactName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public bool? Deleted { get; set; } = false;

    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
