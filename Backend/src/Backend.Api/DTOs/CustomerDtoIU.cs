public class CustomerDtoIU
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public bool? Deleted { get; set; } = false;
    public int AddressId { get; set; }
}