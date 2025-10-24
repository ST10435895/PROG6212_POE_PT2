namespace GUTClaimsApi.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public int UserId { get; set; }
        public int ProgrammeId { get; set; }
        public string MonthYear { get; set; }
        public decimal TotalAmount { get; set; }
        public string CurrentStatus { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public string Remarks { get; set; }
    }
}