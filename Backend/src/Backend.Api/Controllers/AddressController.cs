using Backend.Api.Models;
using Backend.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/adress")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _addressService.GetAllAsync();
            return Ok(addresses);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetById([FromHeader(Name = "Address-Id")] int id)
        {
            var address = await _addressService.GetByIdAsync(id);
            if (address == null) return NoContent();
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Address address)
        {
            var created = await _addressService.CreateAsync(address);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromHeader(Name = "Address-Id")] int id, Address address)
        {
            var updated = await _addressService.UpdateAsync(id, address);
            if (updated == null) return NoContent();
            return Ok(updated);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromHeader(Name = "Address-Id")] int id)
        {
            var deleted = await _addressService.DeleteAsync(id);
            if (!deleted) return NoContent();
            return Ok();
        }
    }
}
