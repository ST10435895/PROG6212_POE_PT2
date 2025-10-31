using CMCS.Api.Data;
using CMCS.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly CmcsDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FilesController(CmcsDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 3️⃣ Upload supporting documents
        [HttpPost("upload/{claimId}")]
        public async Task<IActionResult> UploadFile(int claimId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var allowedTypes = new[] { "application/pdf",
                                       "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                       "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest("Unsupported file type.");

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("File exceeds 10MB limit.");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, Guid.NewGuid() + Path.GetExtension(file.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var document = new SupportingDocument
            {
                ClaimId = claimId,
                FileName = file.FileName,
                FilePath = filePath,
                MimeType = file.ContentType
            };

            _context.SupportingDocuments.Add(document);
            await _context.SaveChangesAsync();

            return Ok(new { document.Id, document.FileName });
        }
    }
}