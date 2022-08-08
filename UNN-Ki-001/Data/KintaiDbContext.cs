using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class KintaiDbContext : DbContext
    {
        public KintaiDbContext(DbContextOptions<KintaiDbContext> options)
            : base(options)
        {
        }

        public KintaiDbContext()
            : base()
        {
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // 変更点をループしreloadメソッドを呼びだす
            var entries = this.ChangeTracker.Entries<Reloadable>();
            foreach(var entry in entries)
            {
                entry.Entity.reload();
            }
            
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.KinmuCd });
            modelBuilder.Entity<T_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo, c.KinmuDt });
        }

        public DbSet<M_Kinmu>? m_kinmus{ get; set; }

        public DbSet<T_Kinmu>? t_kinmus { get; set; }
    }
}
