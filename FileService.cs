using CMCS.Api.Data;
using CMCS.Api.Models;

namespace CMCS.Api.Services
{
    public class FileService : IFileService
    {
        private readonly CmcsDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedTypes =
        {
            "application/pdf",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };

        public FileService(CmcsDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<SupportingDocument> UploadFileAsync(int claimId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            if (!_allowedTypes.Contains(file.ContentType))
                throw new ArgumentException("Unsupported file type.");

            if (file.Length > 10 * 1024 * 1024)
                throw new ArgumentException("File exceeds 10MB limit.");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var doc = new SupportingDocument
            {
                ClaimId = claimId,
                FileName = file.FileName,
                FilePath = filePath,
                MimeType = file.ContentType,
                UploadedAt = DateTime.UtcNow
            };

            _context.SupportingDocuments.Add(doc);
            await _context.SaveChangesAsync();

            return doc;
        }
    }
}