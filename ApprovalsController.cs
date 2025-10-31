using CMCS.Api.Data;
using CMCS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovalsController : ControllerBase
    {
        private readonly CmcsDbContext _context;
        public ApprovalsController(CmcsDbContext context) => _context = context;

        // 2️⃣ Get all pending claims
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingClaims()
        {
            var claims = await _context.Claims
                .Include(c => c.ClaimLines)
                .Where(c => c.Status == "Pending")
                .ToListAsync();

            return Ok(claims);
        }

        // Approve/Reject
        [HttpPost("{claimId}/decision")]
        public async Task<IActionResult> ApproveClaim(int claimId, [FromBody] Approval approval)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null) return NotFound("Claim not found.");

            claim.Status = approval.Decision;
            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Claim {approval.Decision} successfully." });
        }
    }
}