namespace Backend.Api.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public bool? Deleted { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
