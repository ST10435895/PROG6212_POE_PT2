using CMCS.Api.Data;
using CMCS.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMCS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly CmcsDbContext _context;

        public ClaimsController(CmcsDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Submit a claim
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitClaim([FromBody] Claim claim)
        {
            if (claim == null || claim.ClaimLines == null || !claim.ClaimLines.Any())
                return BadRequest("Invalid claim submission.");

            foreach (var line in claim.ClaimLines)
                line.Amount = line.HoursWorked * line.HourlyRate;

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Claim submitted successfully", claim.Id });
        }

        // 4️⃣ Get claim status
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetClaimStatus(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            return Ok(new { claim.Id, claim.Status });
        }
    }
}