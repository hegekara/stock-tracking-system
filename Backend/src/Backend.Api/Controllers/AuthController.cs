using Backend.Api.Dto;
using Backend.Api.Helpers;
using Backend.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                Log.Warning("Kayıt başarısız: {@Errors} | Kullanıcı: {@User}", result.Errors, dto);
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var roles = await _userManager.GetRolesAsync(user);
            var token = JwtGenerator.GenerateToken(user, roles, _config);

            return Ok(new AuthResponseDto
            {
                id = user.Id,
                token = token,
                message = "Kayıt başarılı"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                Log.Warning("Giriş reddedildi - Hatalı şifre: {@Username}", dto.Username);
                return Unauthorized("Şifre yanlış");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = JwtGenerator.GenerateToken(user, roles, _config);

            return Ok(new AuthResponseDto
            {
                id = user.Id,
                token = token,
                message = "Giriş Başarılı"
            });
        }
    }
}
