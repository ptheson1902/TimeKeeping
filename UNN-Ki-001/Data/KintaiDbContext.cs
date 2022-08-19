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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.KinmuCd });
            modelBuilder.Entity<T_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo, c.KinmuDt });
            modelBuilder.Entity<T_Kyukei>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo, c.KinmuDt, c.SeqNo });
        }

        public DbSet<m_kensakushain> m_shains => Set<m_kensakushain>();
        public DbSet<M_Shokushu> m_shokushu => Set<M_Shokushu>();
        public DbSet<M_Shozoku> m_shozoku => Set<M_Shozoku>();
        public DbSet<M_Koyokeitai> koyokeitai => Set<M_Koyokeitai>();
        public DbSet<M_Kinmu> m_kinmus => Set<M_Kinmu>();
        public DbSet<T_Kinmu> t_kinmus => Set<T_Kinmu>();
        public DbSet<T_Kyukei> t_kyukei => Set<T_Kyukei>();
    }
}
