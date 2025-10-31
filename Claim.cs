using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMCS.Api.Data;
using CMCS.Models;

namespace CMCS.Api.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(LecturerProfile))]
        public int LecturerProfileId { get; set; }
        public LecturerProfile LecturerProfile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public ICollection<ClaimLine> ClaimLines { get; set; }
        public ICollection<SupportingDocument> SupportingDocuments { get; set; }
        public ICollection<Approval> Approvals { get; set; }
    }
}