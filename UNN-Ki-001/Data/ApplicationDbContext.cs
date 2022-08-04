using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<M_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.KinmuCd });
        }


        public DbSet<Users> user { get; set; }

        public DbSet<M_Kinmu> m_Kinmus { get; set; }
    }
}