using CMCS.Api.Data;
using CMCS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Api.Services
{
    public class ClaimService
    {
        private readonly CmcsDbContext _context;

        public ClaimService(CmcsDbContext context)
        {
            _context = context;
        }

        public async Task<Claim> SubmitClaimAsync(Claim claim)
        {
            if (claim == null || claim.ClaimLines == null || !claim.ClaimLines.Any())
                throw new ArgumentException("Invalid claim submission.");

            // Calculate amount for each claim line
            foreach (var line in claim.ClaimLines)
                line.Amount = line.HoursWorked * line.HourlyRate;

            claim.CreatedAt = DateTime.UtcNow;
            claim.Status = "Pending";

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return claim;
        }

        public async Task<Claim?> GetClaimByIdAsync(int id)
        {
            return await _context.Claims
                .Include(c => c.ClaimLines)
                .Include(c => c.SupportingDocuments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<string> GetClaimStatusAsync(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            return claim?.Status ?? "Not Found";
        }

        public async Task UpdateClaimStatusAsync(int id, string status)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) throw new KeyNotFoundException("Claim not found.");

            claim.Status = status;
            claim.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}