using CMCS.Api.Data;
using CMCS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Api.Services
{
    public class ApprovalService
    {
        private readonly CmcsDbContext _context;

        public ApprovalService(CmcsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Claim>> GetPendingClaimsAsync()
        {
            return await _context.Claims
                .Include(c => c.ClaimLines)
                .Include(c => c.LecturerProfile)
                .Where(c => c.Status == "Pending")
                .ToListAsync();
        }

        public async Task<Approval> ApproveOrRejectClaimAsync(int claimId, Approval approval)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null) throw new KeyNotFoundException("Claim not found.");

            if (approval.Decision != "Approved" && approval.Decision != "Rejected")
                throw new ArgumentException("Decision must be either 'Approved' or 'Rejected'.");

            claim.Status = approval.Decision;
            approval.ClaimId = claimId;
            approval.DecisionDate = DateTime.UtcNow;

            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();

            return approval;
        }
    }
}