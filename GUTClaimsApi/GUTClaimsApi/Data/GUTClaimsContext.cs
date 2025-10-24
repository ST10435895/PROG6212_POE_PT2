using System.Collections.Generic;
using System.Security.Claims;
using GUTClaimsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GUTClaimsApi.Data
{
    public class GUTClaimsContext : DbContext
    {
        public GUTClaimsContext(DbContextOptions<GUTClaimsContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<System.Security.Claims.Claim> Claims { get; set; }
        public DbSet<ClaimItem> ClaimItems { get; set; }
    }

    public class User
    {
    }

    public class ClaimItem
    {
    }
}