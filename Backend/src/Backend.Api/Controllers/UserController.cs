using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Backend.Api.Models;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users
            .Where(u => !u.Deleted)
            .Select(u => new
            {
                u.Id,
                u.UserName,
                u.FullName,
                u.Email
            }).ToList();

            return Ok(users);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.Deleted)
                return NotFound("Kullanıcı bulunamadı");


            user.Deleted = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                Log.Error("Kullanıcı silinemedi: {@Errors}", result.Errors);
                return BadRequest(result.Errors);
            }

            Log.Warning("Kullanıcı silindi - ID: {Id} - {FirstName} {LastName}", id, user.FullName);
            return Ok("Kullanıcı silindi");
        }
    }
}
