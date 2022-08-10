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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // M_KINMUレコードの変更があれば、
            // 紐づいたT_KINMUレコードも再計算にエントリーさせる
            var mKinmus = ChangeTracker.Entries<M_Kinmu>();
            if(mKinmus.Count() > 0)
            {
                // KinmuCdのリストを取得
                List<string> list = new List<string>();
                foreach(var mKinmu in mKinmus)
                {
                    list.Add(mKinmu.Entity.KinmuCd);
                }

                // 該当するT_KINMUレコードを取得
                var tKinmus = t_kinmus.Where(e => e.KinmuCd != null && list.Contains(e.KinmuCd));

                // Reloadableの対象としてマーク
                foreach(var tKinmu in tKinmus)
                {
                    Entry(tKinmu).State = EntityState.Modified;
                }

            }

            // Reloadableを実行
            var records = ChangeTracker.Entries<Reloadable>();
            foreach(var record in records)
            {
                record.Entity.reload(this);
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

        public DbSet<m_kensakushain> m_shains => Set<m_kensakushain>();
        public DbSet<M_Shokushu> m_test => Set<M_Shokushu>();
        public DbSet<M_Shozoku> shozoku => Set<M_Shozoku>();
        public DbSet<M_Koyokeitai> koyokeitai => Set<M_Koyokeitai>();
        public DbSet<M_Kinmu> m_kinmus => Set<M_Kinmu>();
        public DbSet<T_Kinmu> t_kinmus => Set<T_Kinmu>();
    }
}
