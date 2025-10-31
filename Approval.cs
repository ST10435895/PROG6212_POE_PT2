using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMCS.Models;

namespace CMCS.Api.Models
{
    public class Approval
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Claim))]
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Decision { get; set; } // Approved or Rejected

        public string? Remarks { get; set; }

        public DateTime DecisionDate { get; set; } = DateTime.UtcNow;
    }
}