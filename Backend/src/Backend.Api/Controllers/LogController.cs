using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/logs")]
    [Authorize(Roles = "Admin")]
    public class LogController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public LogController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("{fileName}")]
        public IActionResult DownloadLog(string fileName)
        {
            Console.WriteLine("indirme isteği geldi");
            var logDir = Path.Combine(_env.ContentRootPath, "Logs");
            var filePath = Path.Combine(logDir, fileName);

            Console.WriteLine(filePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Log dosyası bulunamadı.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/plain", fileName);
        }

        [HttpGet("list")]
        public IActionResult ListLogs()
        {

            Console.WriteLine("listeleme isteği geldi");
            var logDir = Path.Combine(_env.ContentRootPath, "Logs");

            if (!Directory.Exists(logDir))
                return NotFound("Logs klasörü bulunamadı.");

            var files = Directory.GetFiles(logDir)
                                 .Select(Path.GetFileName)
                                 .ToList();

            return Ok(files);
        }
    }
}
