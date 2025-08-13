namespace Backend.Api.Models;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool? Deleted { get; set; } = false;

    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;

    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
