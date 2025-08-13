namespace Backend.Api.Models;

public enum OrderStatus
{
    Ordered,
    Pending,
    Delivery,
    Completed,
    Aborted
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Ordered;
    public bool? Deleted { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
