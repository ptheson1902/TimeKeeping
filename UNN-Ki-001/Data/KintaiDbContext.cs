using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class KintaiDbContext : DbContext
    {
        public KintaiDbContext(DbContextOptions<KintaiDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            int count = 99;
            List<string> list = new List<string>();
            while(count != 0)
            {
                count = 0;
                foreach(var record in ChangeTracker.Entries<Reloadable>().ToList())
                {
                    if (!record.Entity.isReloaded)
                    {
                        count++;
                        Debug.Write("リロード実行" + record.ToString());
                        record.Entity.run(this);
                    }
                }
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 複合プライマリーキーの設定
            modelBuilder.Entity<M_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.KinmuCd });
            modelBuilder.Entity<T_Kinmu>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo, c.KinmuDt });
            modelBuilder.Entity<T_Kyukei>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo, c.KinmuDt, c.SeqNo });
            modelBuilder.Entity<M_Shain>()
                .HasKey(c => new { c.KigyoCd, c.ShainNo });
            modelBuilder.Entity<M_Shozoku>()
                .HasKey(c => new { c.KigyoCd, c.ShozokuCd });
            modelBuilder.Entity<M_Shokushu>()
                .HasKey(c => new { c.KigyoCd, c.ShokushuCd });
            modelBuilder.Entity<M_Koyokeitai>()
                .HasKey(c => new { c.KigyoCd, c.KoyokeitaiCd });

            // 複合外部キーの設定
            modelBuilder.Entity<M_Shain>()
                .HasOne(shain => shain.Koyokeitai)
                .WithMany(koyokeitai => koyokeitai.Shains)
                .HasForeignKey(shain => new { shain.KigyoCd, shain.KoyokeitaiCd });
            modelBuilder.Entity<M_Shain>()
                .HasOne(shain => shain.Shokushu)
                .WithMany(shokushu => shokushu.Shains)
                .HasForeignKey(shain => new { shain.KigyoCd, shain.ShokushuCd });
            modelBuilder.Entity<M_Shain>()
                .HasOne(shain => shain.Shozoku)
                .WithMany(shozoku => shozoku.Shains)
                .HasForeignKey(shain => new { shain.KigyoCd, shain.ShozokuCd });
            modelBuilder.Entity<T_Kinmu>()
                .HasOne(kinmu => kinmu.MKinmu)
                .WithMany(mKinmu => mKinmu.TKinmus)
                .HasForeignKey(kinmu => new { kinmu.KigyoCd, kinmu.KinmuCd });
            modelBuilder.Entity<T_Kinmu>()
                .HasMany(kinmu => kinmu.TKyukeis)
                .WithOne(kyukei => kyukei.TKinmu)
                .HasForeignKey(kinmu => new { kinmu.KigyoCd, kinmu.ShainNo, kinmu.KinmuDt });
        }

        public DbSet<T_Kyukei> t_Kyukeis => Set<T_Kyukei>();
        public DbSet<M_Shain> m_shains => Set<M_Shain>();
        public DbSet<M_Shokushu> m_shokushus => Set<M_Shokushu>();
        public DbSet<M_Settings> mSettings => Set<M_Settings>();
        public DbSet<M_Shozoku> m_shozokus => Set<M_Shozoku>();
        public DbSet<M_Koyokeitai> m_koyokeitais => Set<M_Koyokeitai>();
        public DbSet<M_Kinmu> m_kinmus => Set<M_Kinmu>();
        public DbSet<T_Kinmu> t_kinmus => Set<T_Kinmu>();
    }
}
