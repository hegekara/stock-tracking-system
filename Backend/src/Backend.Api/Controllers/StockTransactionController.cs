using Backend.Api.Dtos;
using Backend.Api.Models;
using Backend.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class StockTransactionController : ControllerBase
    {
        private readonly IStockTransactionService _service;

        public StockTransactionController(IStockTransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _service.GetAllAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await _service.GetByIdAsync(id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpPost("add-stock")]
        public async Task<ActionResult<StockTransaction>> AddStock([FromBody] StockTransactionDtoIU dto)
        {
            var result = await _service.AddStockAsync(dto);
            return Ok(result);
        }

        [HttpPost("remove-stock")]
        public async Task<ActionResult<StockTransaction>> RemoveStock([FromBody] StockTransactionDtoIU dto)
        {
            var result = await _service.RemoveStockAsync(dto);
            return Ok(result);
        }
    }
}
