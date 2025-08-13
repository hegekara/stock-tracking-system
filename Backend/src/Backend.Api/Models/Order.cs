namespace Backend.Api.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Ordered;
    public bool? Deleted { get; set; } = false;
    public Customer Customer { get; set; } = null!;
}
