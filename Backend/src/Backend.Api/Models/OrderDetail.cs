namespace Backend.Api.Models;

public class OrderDetail
{
    public int Id { get; set;}
    public virtual Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool? Deleted { get; set; } = false;
}
