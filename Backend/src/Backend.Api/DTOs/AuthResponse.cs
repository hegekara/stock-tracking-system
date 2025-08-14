using System.ComponentModel.DataAnnotations;

namespace Backend.Api.Dto
{
    public class AuthResponseDto
    {
        public string? id { get; set; }
        public string? token { get; set; }
        public string? role { get; set; } 
        public string message { get; set; } = "";
    }
}
