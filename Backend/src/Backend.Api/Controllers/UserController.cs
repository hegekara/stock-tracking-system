using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Backend.Api.Models;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    //[Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


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

        [HttpPut("change-role/{id}")]
        public async Task<IActionResult> ChangeUserRole(string id, [FromBody] RoleChangeRequest request)
        {
            var newRole = request.Role;
            var user = await _userManager.FindByIdAsync(id);
            Console.WriteLine(user);
            if (user == null || user.Deleted)
                return NotFound("Kullanıcı bulunamadı");

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return BadRequest(removeResult.Errors);
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                return BadRequest(addResult.Errors);
            }

            return Ok($"Kullanıcının rolü {newRole} olarak değiştirildi");
        }


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
