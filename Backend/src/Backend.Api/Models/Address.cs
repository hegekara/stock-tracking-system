namespace Backend.Api.Models;

public class Address
{
    public int Id { get; set; }

    public string? Street { get; set; }
    public bool? Deleted { get; set; } = false;

    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public int DistrictId { get; set; }
    public District District { get; set; } = null!;
}
