namespace Backend.Api.Dtos
{
    public class ProductDtoUI
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
    }
}
