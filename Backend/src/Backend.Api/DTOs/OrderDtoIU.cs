namespace Backend.Api.DTOs
{
    public class OrderDtoIU
    {
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;
        public bool? Deleted { get; set; } = false;
        public int CustomerId { get; set; }
    }
}
