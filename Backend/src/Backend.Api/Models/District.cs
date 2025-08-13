namespace Backend.Api.Models;

public class District
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Street { get; set; }
    public bool? Deleted { get; set; } = false;

    public int CityId { get; set; }
    public City City { get; set; } = null!;
}
