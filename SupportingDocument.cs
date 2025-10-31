using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Api.Models
{
    public class SupportingDocument
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Claim))]
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string MimeType { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}