using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kyukei", Schema = "public")]
    public class T_Kyukei : Reloadable
    {
        protected override void Reload(KintaiDbContext context)
        {
            Check(context);
        }

        private void Check(KintaiDbContext context)
        {
            // 完成した休憩記録なら
            if (DakokuToDate != null && DakokuFrDate != null)
            { 
                // 開始時間と終了時間の関係を確認
                if (DakokuToDate < DakokuFrDate)
                    throw new Exception("休憩開始時刻と休憩終了時刻の関係が不正です。\n開始時刻=" + DakokuFrDate + "\n終了時刻=" + DakokuToDate);

                // 休憩時間を計算
                Kyukei = (int)(((DateTime)DakokuToDate) - ((DateTime)DakokuFrDate)).TotalMinutes;

                // 姉妹レコードと時間が重複していないかどうかの確認
                var list = context.t_Kyukeis
                    .Where(e => e.KigyoCd.Equals(KigyoCd) && e.ShainNo.Equals(ShainNo) && e.KinmuDt.Equals(KinmuDt) && e.DakokuFrDate != null && e.DakokuToDate != null)
                    .OrderBy(e => e.DakokuFrDate)
                    .AsNoTracking()
                    .ToList();

                // チェンジトラッカーからも取得
                var list2 = context.ChangeTracker.Entries<T_Kyukei>()
                    .Where(e => e.Entity.KigyoCd.Equals(KigyoCd) && e.Entity.ShainNo.Equals(ShainNo) && e.Entity.KinmuDt.Equals(KinmuDt) && e.Entity.DakokuFrDate != null && e.Entity.DakokuToDate != null
                    );

                // 合体
                foreach(var item in list2)
                {
                    list.Add(item.Entity);
                }

                foreach(var item in list)
                {
/*                    // 削除中なら無視
                    if (context.Entry(item).State == EntityState.Deleted)
                    {
                        Debug.WriteLine(context.Entry(item).State);
                        continue;
                    }*/

                    // 開始時間と終了時間、少なくともどちらかが重複している場合例外をスロー
                    if((DakokuFrDate < item.DakokuFrDate && item.DakokuFrDate < DakokuToDate) ||
                        (DakokuFrDate < item.DakokuToDate && item.DakokuToDate < DakokuToDate))
                    {
                        throw new Exception("休憩時間が重複してしまいます。", new Exception(item.KinmuDt));
                    }
                }
            }

            // 勤務レコードの再計算をトリガーする
            var kinmu = context.t_kinmus
                .Where(e => e.KigyoCd!.Equals(KigyoCd) && e.ShainNo!.Equals(ShainNo) && e.KinmuDt!.Equals(KinmuDt))
                .ToList()
                .FirstOrDefault();

            if (kinmu != null)
            {
                context.Attach((T_Kinmu)kinmu);
            }
        }

        public T_Kyukei()
        {

        }

        public override bool Equals(object? target)
        {
            if (target == null || this.GetType() != target.GetType())
            {
                return false;
            }

            T_Kyukei kyukei = (T_Kyukei)target;

            if (kyukei.KigyoCd == KigyoCd && kyukei.ShainNo == ShainNo && kyukei.KinmuDt == KinmuDt && kyukei.SeqNo == SeqNo)
            {
                return true;
            }

            return false;
        }

        public T_Kyukei(string kigyoCd, string shainNo, string kinmuDt, int seqNo, T_Kinmu kinmu)
        {
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
            KinmuDt = kinmuDt;
            SeqNo = seqNo;
            TKinmu = kinmu;
        }

        [Key]
        [Column("kigyo_cd")]
        public string KigyoCd { get; }

        [Key]
        [Column("shain_no")]
        public string ShainNo { get; }

        [Key]
        [Column("kinmu_dt")]
        public string KinmuDt { get; }

        [Key]
        [Column("seq_no")]
        public int SeqNo { get; }

        [Column("dakoku_fr_date")]
        public DateTime? DakokuFrDate { get; set; }

        [Column("dakoku_to_date")]
        public DateTime? DakokuToDate { get; set; }

        [Column("kyukei")]
        public int? Kyukei { get; set; }

        [Column("create_dt")]
        public DateTime? CreateDt { get; }

        [Column("create_usr")]
        public string? CreateUsr { get; set; }

        [Column("create_pgm")]
        public string? CreatePgm { get; set; }

        [Column("update_dt")]
        public DateTime? UpdateDt { get; set; }

        [Column("update_usr")]
        public string? UpdateUsr { get; set; }

        [Column("update_pgm")]
        public string? UpdatePgm { get; set; }


        public T_Kinmu TKinmu { get; set; }
    }
}
