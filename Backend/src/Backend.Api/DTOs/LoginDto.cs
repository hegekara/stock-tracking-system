using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Dto
{
    public class LoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
