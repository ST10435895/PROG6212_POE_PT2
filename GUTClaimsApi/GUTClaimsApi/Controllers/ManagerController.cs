using GUTClaimsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GUTClaimsApi.Controllers
{
    [ApiController]
    [Route("api/manager")]
    public class ManagerController : ControllerBase
    {
        private readonly ClaimService _service;
        public ManagerController(ClaimService service) => _service = service;

        [HttpPut("approve/{claimId}")]
        public async Task<IActionResult> Approve(int claimId, int changedBy)
        {
            await _service.UpdateStatusAsync(claimId, "Approved", changedBy, "Approved by Manager");
            return Ok("Claim approved successfully");
        }

        [HttpPut("settle/{claimId}")]
        public async Task<IActionResult> Settle(int claimId, int changedBy)
        {
            await _service.UpdateStatusAsync(claimId, "Settled", changedBy, "Payment completed");
            return Ok("Claim settled");
        }
    }
}