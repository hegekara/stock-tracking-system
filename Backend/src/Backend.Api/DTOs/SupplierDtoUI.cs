namespace Backend.Api.Dtos
{
    public class SupplierDtoIU
    {
        public string Name { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public int AddressId { get; set; }
    }
}