using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Dto
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
