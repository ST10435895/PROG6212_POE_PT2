using GUTClaimsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GUTClaimsApi.Controllers
{
    [ApiController]
    [Route("api/document")]
    public class DocumentController : ControllerBase
    {
        private readonly DbConnectionHelper _db;
        private readonly IWebHostEnvironment _env;
        public DocumentController(DbConnectionHelper db, IWebHostEnvironment env)
        {
            _db = db; _env = env;
        }

        [HttpPost("upload/{claimId}")]
        public async Task<IActionResult> Upload(int claimId, IFormFile file, int uploadedBy)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            var allowed = new[] { ".pdf", ".docx", ".xlsx" };
            if (!allowed.Contains(ext))
                return BadRequest("Invalid file type.");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
            using (var stream = System.IO.File.Create(path))
                await file.CopyToAsync(stream);

            using var conn = _db.GetConnection();
            await conn.OpenAsync();
            var cmd = new SqlCommand(@"INSERT INTO SupportingDocument(ClaimId, FileName, FileUrl, UploadedBy)
                                       VALUES(@c,@f,@u,@b)", conn);
            cmd.Parameters.AddWithValue("@c", claimId);
            cmd.Parameters.AddWithValue("@f", file.FileName);
            cmd.Parameters.AddWithValue("@u", "/uploads/" + fileName);
            cmd.Parameters.AddWithValue("@b", uploadedBy);
            await cmd.ExecuteNonQueryAsync();

            return Ok("File uploaded successfully");
        }
    }
}