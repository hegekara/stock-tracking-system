namespace Backend.Api.Models;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool? Deleted { get; set; } = false;
    
    public Address Address { get; set; } = null!;
}
