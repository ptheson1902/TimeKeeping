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
            // 勤務レコードをセレクト
            T_Kinmu? kinmu = context.t_kinmus.Where(e =>
                e.KigyoCd.Equals(KigyoCd)
                && e.ShainNo.Equals(ShainNo)
                && e.KinmuDt.Equals(KinmuDt))
                .FirstOrDefault();

            // -開始時刻のチェック
            if (DakokuFrDate != null)
            {
                // 出退勤記録に合わせて変形
                if (kinmu != null)
                {
                    if (kinmu.KinmuFrDate != null && kinmu.KinmuFrDate > DakokuFrDate)
                        DakokuFrDate = ((DateTime)kinmu.KinmuFrDate).ToUniversalTime();
                }


                // 既存レコードの中から直前の完成した休憩レコードを取得
                var tgt1 = context.t_Kyukeis.Where(e =>
                    (!e.KinmuDt.Equals(KinmuDt) || !e.SeqNo.Equals(SeqNo))                  // このレコード以外で
                    && e.KigyoCd.Equals(KigyoCd)                                            // 同一社員の...
                    && e.ShainNo.Equals(ShainNo)                                            //
                    && e.DakokuFrDate != null                                               // 休憩終了まで完了した休憩レコードの内...
                    && e.DakokuToDate != null                                               //
                    && e.DakokuFrDate <= ((DateTime)DakokuFrDate).ToUniversalTime())         // このレコード以前の休憩時間レコードの
                    .OrderByDescending(e => e.DakokuFrDate)                                 // 休憩開始時間の降順で
                    .FirstOrDefault();                                                      // １件目

                // チェンジトラッカーの中から直前の完成した休憩レコードを取得
                var tgt2 = context.ChangeTracker.Entries<T_Kyukei>().Where(e =>
                    (!e.Entity.KinmuDt.Equals(KinmuDt) || !e.Entity.SeqNo.Equals(SeqNo))    // このレコード以外で
                    && e.Entity.KigyoCd.Equals(KigyoCd)                                     // 同一社員の...
                    && e.Entity.ShainNo.Equals(ShainNo)                                     //
                    && e.Entity.DakokuFrDate != null                                        // 休憩終了まで完了した休憩レコードの内...
                    && e.Entity.DakokuToDate != null                                        //
                    && e.Entity.DakokuFrDate <= ((DateTime)DakokuFrDate).ToUniversalTime())  // このレコード以前の休憩時間レコードの
                    .OrderByDescending(e => e.Entity.DakokuFrDate)                          // 休憩開始時間の降順で
                    .FirstOrDefault();                                                      // １件目
                // 検証対象のリストを作成
                List<T_Kyukei> list = new List<T_Kyukei>();
                if (tgt1 != null)
                    list.Add(tgt1);
                if (tgt2 != null)
                    list.Add(tgt2.Entity);
                // 休憩時間が重複していないかの検証を実行
                foreach (var item in list)
                {
                    if (item.DakokuToDate > DakokuFrDate)           // ターゲットの休憩終了時刻がこのレコードの休憩開始時刻より後だった場合
                    {
                        throw new Exception("休憩開始時間が直前のレコードの休憩時間と重複しています。");
                    }
                }
            }

            // -終了時刻のチェック
            if (DakokuToDate != null)
            {
                // 出退勤記録に合わせて変形
                if (kinmu != null)
                {
                    if (kinmu.KinmuToDate != null && kinmu.KinmuToDate < DakokuToDate)
                        DakokuToDate = ((DateTime)kinmu.KinmuToDate).ToUniversalTime();
                }

                // 既存レコードの中から直後の完成した休憩レコードを取得
                var tgt1 = context.t_Kyukeis.Where(e =>
                    (!e.KinmuDt.Equals(KinmuDt) || !e.SeqNo.Equals(SeqNo))                  // このレコード以外で
                    && e.KigyoCd.Equals(KigyoCd)                                            // 同一社員の...
                    && e.ShainNo.Equals(ShainNo)                                            //
                    && e.DakokuFrDate != null                                               // 休憩終了まで完了した休憩レコードの内...
                    && e.DakokuToDate != null                                               //
                    && e.DakokuToDate >= ((DateTime)DakokuToDate).ToUniversalTime())         // このレコード以降の休憩時間レコードの
                    .OrderBy(e => e.DakokuToDate)                                           // 休憩開始時間の昇順で
                    .FirstOrDefault();                                                      // １件目

                // チェンジトラッカーの中から直後の完成した休憩レコードを取得
                var tgt2 = context.ChangeTracker.Entries<T_Kyukei>().Where(e =>
                    (!e.Entity.KinmuDt.Equals(KinmuDt) || !e.Entity.SeqNo.Equals(SeqNo))    // このレコード以外で
                    && e.Entity.KigyoCd.Equals(KigyoCd)                                     // 同一社員の...
                    && e.Entity.ShainNo.Equals(ShainNo)                                     //
                    && e.Entity.DakokuFrDate != null                                        // 休憩終了まで完了した休憩レコードの内...
                    && e.Entity.DakokuToDate != null                                        //
                    && e.Entity.DakokuToDate >= ((DateTime)DakokuToDate).ToUniversalTime())  // このレコード以前の休憩時間レコードの
                    .OrderBy(e => e.Entity.DakokuToDate)                                    // 休憩開始時間の降順で
                    .FirstOrDefault();                                                      // １件目
                // 検証対象のリストを作成
                List<T_Kyukei> list = new List<T_Kyukei>();
                if (tgt1 != null)
                    list.Add(tgt1);
                if (tgt2 != null)
                    list.Add(tgt2.Entity);
                // 休憩時間が重複していないかの検証を実行
                foreach (var item in list)
                {
                    if (item.DakokuFrDate < DakokuToDate)           // ターゲットの休憩終了時刻がこのレコードの休憩開始時刻より後だった場合
                    {
                        throw new Exception("休憩終了時間が直後のレコードの休憩時間と重複しています。");
                    }
                }
            }

            // 開始時間と終了時間のチェック
            if (DakokuToDate != null && DakokuFrDate != null)
            {
                if (DakokuToDate <= DakokuFrDate)
                    throw new Exception("休憩開始時刻と休憩終了時刻の関係が不正です。");
            }
        }

        public T_Kyukei(string kigyoCd, string shainNo, string kinmuDt, int seqNo)
        {
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
            KinmuDt = kinmuDt;
            SeqNo = seqNo;
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

    }
}
