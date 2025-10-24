using GUTClaimsApi.Data;
using Microsoft.EntityFrameworkCore;

private readonly GUTClaimsContext _context;

public static LecturerClaimController(GUTClaimsContext context)
{
    _context = context;
    _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    if (!Directory.Exists(_uploadFolder))
        Directory.CreateDirectory(_uploadFolder);
}