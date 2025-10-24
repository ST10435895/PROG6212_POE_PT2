using GUTClaimsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GUTClaimsApi.Controllers
{
    [ApiController]
    [Route("api/coordinator")]
    public class CoordinatorController : ControllerBase
    {
        private readonly ClaimService _service;
        public CoordinatorController(ClaimService service) => _service = service;

        [HttpPut("verify/{claimId}")]
        public async Task<IActionResult> Verify(int claimId, int changedBy, string comment)
        {
            await _service.UpdateStatusAsync(claimId, "Verified", changedBy, comment);
            return Ok("Claim verified successfully");
        }

        [HttpPut("reject/{claimId}")]
        public async Task<IActionResult> Reject(int claimId, int changedBy, string comment)
        {
            await _service.UpdateStatusAsync(claimId, "Rejected", changedBy, comment);
            return Ok("Claim rejected");
        }
    }
}