using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Api.Models
{
    public class ClaimLine
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Claim))]
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        [Required]
        public decimal HoursWorked { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}