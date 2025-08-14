using Microsoft.AspNetCore.Identity;

namespace Backend.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = null!;
    public bool Deleted { get; set; } = false;
}

