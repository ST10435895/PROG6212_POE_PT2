using System.Collections.Generic;
using CMCS.Api.Models;
using CMCS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Api.Data
{
    public class CmcsDbContext : DbContext
    {
        public CmcsDbContext(DbContextOptions<CmcsDbContext> options) : base(options) { }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimLine> ClaimLines { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<LecturerProfile> LecturerProfiles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}