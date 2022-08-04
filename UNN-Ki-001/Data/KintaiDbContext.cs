using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class KintaiDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.KinmuCd });
            modelBuilder.Entity<T_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo, c.KinmuDt });
        }
    }
}
