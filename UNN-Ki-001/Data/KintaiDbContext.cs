using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class KintaiDbContext : DbContext
    {
        public KintaiDbContext(DbContextOptions<KintaiDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.KinmuCd });
            modelBuilder.Entity<T_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo, c.KinmuDt });
        }

        public DbSet<M_Kinmu> M_Kinmu { get; set; }
        public DbSet<T_Kinmu> T_Kinmu { get; set; }
    }
}
