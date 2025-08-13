namespace Backend.Api.Models;

public class Address
{
    public int Id { get; set; }
    public string? CityName { get; set; }
    public string? DistrictName { get; set; }
    public bool Deleted { get; set; } = false;
}
