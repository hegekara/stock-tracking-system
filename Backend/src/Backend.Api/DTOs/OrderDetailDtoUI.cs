namespace Backend.Api.DTOs
{
    public class OrderDetailCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}