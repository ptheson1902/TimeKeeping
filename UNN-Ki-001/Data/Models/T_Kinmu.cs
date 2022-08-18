using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kinmu", Schema = "public")]
    public class T_Kinmu : Reloadable
    {
        public T_Kinmu(string kigyoCd, string shainNo, string kinmuDt)
        {
            // 必須項目を入力
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
            KinmuDt = kinmuDt;
        }

        
        protected override void Reload(KintaiDbContext context)
        {

            // マスターレコードの取得
            M_Kinmu? master = context.m_kinmus
                .Where(e => e.KinmuCd.Equals(KinmuCd) && e.KigyoCd.Equals(KigyoCd))
                .FirstOrDefault();

            // 実績開始時間の計算
            if(DakokuFrDate != null && KinmuFrDate == null)
            {
                KinmuFrDate = CalcKinmuFr(master, (DateTime)DakokuFrDate);
            }

            // 実績終了時間の計算
            if(DakokuToDate != null && KinmuToDate == null)
            {
                KinmuToDate = CalcKinmuTo(master, (DateTime)DakokuToDate);
            }

            // 直前の勤務レコードと重複していないかどうかの確認
            if (KinmuFrDate != null)
            {
                var tgt = context.t_kinmus
                    .Where(e => e.KigyoCd.Equals(KigyoCd) && e.ShainNo.Equals(ShainNo) && e.KinmuFrDate != null && e.KinmuToDate != null && e.KinmuFrDate < ((DateTime)KinmuFrDate).ToUniversalTime())
                    .OrderByDescending(e => e.KinmuFrDate)
                    .FirstOrDefault();
                if(tgt != null && tgt.KinmuToDate > KinmuFrDate)
                {
                    throw new Exception("直前の勤務記録と重複してしまいます。\n丸め処理等を改めるか、打刻時間等を見直す必要があります。");
                }
            }
            // 直後の勤務レコードと重複していないかどうかの確認
            if (KinmuToDate != null)
            {
                var tgt = context.t_kinmus
                    .Where(e => e.KigyoCd.Equals(KigyoCd) && e.ShainNo.Equals(ShainNo) && e.KinmuFrDate != null && e.KinmuToDate != null && e.KinmuToDate >((DateTime)KinmuToDate).ToUniversalTime())
                    .OrderBy(e => e.KinmuToDate)
                    .FirstOrDefault();
                if (tgt != null && tgt.KinmuFrDate < KinmuToDate)
                {
                    throw new Exception("直後の勤務記録と重複してしまいます。\n丸め処理等を改めるか、打刻時間等を見直す必要があります。");
                }
            }
            // 実績時間の整合性を確認・修正
            if (KinmuFrDate != null && KinmuToDate != null)
            {
                // 総労働時間がマイナスになるようなら訂正する(丸め処理によっては必要になる）
                if(KinmuFrDate > KinmuToDate)
                {
                    KinmuToDate = KinmuFrDate;
                }
            }
            
            // 所定時間の計算
            if(Shotei == null)
            {
                Shotei = CalcShotei(master);
            }

            // 休憩処理
            if(KinmuFrDate != null && KinmuToDate != null)
            {
                // 休憩時間の自動追加(総休憩時間がNULLの場合にのみ動作させます。)
                if (Kyukei == null && master != null && master.KyukeiAutoFlg != null && master.KyukeiAutoFlg.Equals("1"))
                {
                    AddKyukeiFromMaster((M_Kinmu)master, context);
                }

                // 休憩時間の計算
                Kyukei = CalcKyukeiTotalMinutes(context, (DateTime)KinmuFrDate, (DateTime)KinmuToDate);
            }

            // その他集計処理
            if(KinmuFrDate != null && KinmuToDate != null)
            {
                // 総労働時間の計算
                Sorodo = (int)((DateTime)KinmuToDate - (DateTime)KinmuFrDate).TotalMinutes - Kyukei;

                // 控除時間の計算
                Kojo = Shotei - Sorodo;
                if(Kojo < 0)
                {
                    Kojo = 0;
                }
            }
        }

        private DateTime CalcKinmuFr(M_Kinmu? master, DateTime dakokuFrDate)
        {
            if(master == null)
            {
                return dakokuFrDate;
            }

            // 丸め処理を行う
            DateTime result = new DateControl(dakokuFrDate).MarumeProcess(master.KinmuFrMarumeTm, master.KinmuFrMarumeKbn).Origin;

            // 刻限の適用
            if (master.KinmuFrTm != null && master.KinmuFrKbn != null && master.KinmuFrCtrlFlg != null)
            {
                DateTime ZissekiFrDate = new DateControl(KinmuDt, master.KinmuFrTm, master.KinmuFrKbn).Origin;
                if (ZissekiFrDate > result)
                {
                    result = ZissekiFrDate;
                }
            }

            return result;
        }

        private DateTime CalcKinmuTo(M_Kinmu? master, DateTime dakokuToDate)
        {
            if(master == null)
            {
                return dakokuToDate;
            }

            // 丸め処理を行う
            DateTime result = new DateControl(dakokuToDate).MarumeProcess(master.KinmuToMarumeTm, master.KinmuToMarumeKbn).Origin;

            return result;
        }

        private int CalcShotei(M_Kinmu? master)
        {
            if(master == null || master.ShoteiTm == null)
            {
                return 0;
            }

            return (int)master.ShoteiTm;
        }

        private void AddKyukeiFromMaster(M_Kinmu master, KintaiDbContext context)
        {
            // 既存の休憩をすべて削除
            // 手打ちで入力した休憩レコードと自動追加されたものの見分けが付かないため、問答無用で消去する仕様にした。
            var target = context.t_Kyukeis
                .Where(e => e.KigyoCd.Equals(KigyoCd) && e.ShainNo.Equals(ShainNo) && e.KinmuDt.Equals(KinmuDt));
            context.RemoveRange(target);


            List<T_Kyukei> list = new List<T_Kyukei>();
            int count = 0;

            // 休憩１を生成・リストに追加
            if (master.Kyukei1FrKbn != null && master.Kyukei1FrTm != null && master.Kyukei1ToKbn != null && master.Kyukei1ToTm != null)
            {
                DateControl frDc = new DateControl(KinmuDt, master.Kyukei1FrTm, master.Kyukei1FrKbn);
                DateControl toDc = new DateControl(KinmuDt, master.Kyukei1ToTm, master.Kyukei1ToKbn);
                T_Kyukei kyukei = new T_Kyukei(KigyoCd, ShainNo, KinmuDt, ++count);
                kyukei.DakokuFrDate = frDc.Origin;
                kyukei.DakokuToDate = toDc.Origin;
                list.Add(kyukei);
                Debug.WriteLine(kyukei.DakokuFrDate.ToString());
            }
            // 休憩２を生成・リストに追加
            if (master.Kyukei2FrKbn != null && master.Kyukei2FrTm != null && master.Kyukei2ToKbn != null && master.Kyukei2ToTm != null)
            {
                DateControl frDc = new DateControl(KinmuDt, master.Kyukei2FrTm, master.Kyukei2FrKbn);
                DateControl toDc = new DateControl(KinmuDt, master.Kyukei2ToTm, master.Kyukei2ToKbn);
                T_Kyukei kyukei = new T_Kyukei(KigyoCd, ShainNo, KinmuDt, ++count);
                kyukei.DakokuFrDate = frDc.Origin;
                kyukei.DakokuToDate = toDc.Origin;
                list.Add(kyukei);
                Debug.WriteLine(kyukei.DakokuFrDate.ToString());
            }
            
            // 休憩３を生成・リストに追加
            if (master.Kyukei3FrKbn != null && master.Kyukei3FrTm != null && master.Kyukei3ToKbn != null && master.Kyukei3ToTm != null)
            {
                DateControl frDc = new DateControl(KinmuDt, master.Kyukei3FrTm, master.Kyukei3FrKbn);
                DateControl toDc = new DateControl(KinmuDt, master.Kyukei3ToTm, master.Kyukei3ToKbn);
                T_Kyukei kyukei = new T_Kyukei(KigyoCd, ShainNo, KinmuDt, ++count);
                kyukei.DakokuFrDate = frDc.Origin;
                kyukei.DakokuToDate = toDc.Origin;
                list.Add(kyukei);
            }

            context.t_Kyukeis.AddRange(list);
        }

        /// <summary>
        /// 勤務レコードに紐づいた休憩レコードの内、勤務実績時間の枠に収まっているものの総合時間を取得する。
        /// 勤務時間からはみ出た分はカットして計算する。
        /// </summary>
        /// <param name="context">NotNullable</param>
        /// <param name="kinmuFrDate">NotNullable</param>
        /// <param name="kinmuToDate">NotNullable</param>
        /// <returns></returns>
        private int CalcKyukeiTotalMinutes(KintaiDbContext context, DateTime kinmuFrDate, DateTime kinmuToDate)
        {
            // 紐づいている全ての休憩レコードを取得する
            var list = context.t_Kyukeis
                .Where(e => e.KigyoCd.Equals(KigyoCd) && e.ShainNo.Equals(ShainNo) && e.KinmuDt.Equals(KinmuDt)
                    && e.DakokuFrDate != null && e.DakokuToDate != null)
                .OrderBy(e => e.SeqNo)
                .ToList();

            int totalMinutes = 0;

            Debug.WriteLine("数は" + list.Count);

            // 勤務時間の枠に押し込む
            foreach (var item in list)
            {
                // コンパイラにNULLじゃないことを保証します
                if(item.DakokuToDate == null || item.DakokuFrDate == null)
                {
                    continue;
                }

                // 休憩時間を勤務実績時間の枠に押し込める
                if (item.DakokuFrDate < kinmuFrDate)
                {
                    item.DakokuFrDate = kinmuFrDate;
                }
                if (item.DakokuToDate > kinmuToDate)
                {
                    item.DakokuToDate = kinmuToDate;
                }
                // 結果として意味を喪失していないレコードのみを選択して休憩時間を計算
                if (item.DakokuFrDate < item.DakokuToDate)
                {
                    totalMinutes += (int)((DateTime)item.DakokuToDate - (DateTime)item.DakokuFrDate).TotalMinutes;
                }
            }
            return totalMinutes;
        }

        private void ClearInfo()
        {
            Shotei = null;
            Kyukei = null;
            Hoteigai = null;
            Hoteinai = null;
            Sorodo = null;
        }

        public void SetKinmuCd(string kinmuCd)
        {
            ClearInfo();
            KinmuCd = kinmuCd;
        }

        [Key]
        [Column("kigyo_cd")]
        public string KigyoCd { get; set; }

        [Key]
        [Column("shain_no")]
        public string ShainNo { get; set; }

        [Key]
        [Column("kinmu_dt")]
        public string KinmuDt { get; set; }

        [Column("kinmu_cd")]
        public string? KinmuCd { get; private set; }

        [Column("dakoku_fr_date")]
        public DateTime? DakokuFrDate { get; set; }

        [Column("dakoku_to_date")]
        public DateTime? DakokuToDate { get; set; }

        [Column("kinmu_fr_date")]
        public DateTime? KinmuFrDate { get; private set; }

        [Column("kinmu_to_date")]
        public DateTime? KinmuToDate { get; private set; }

        [Column("dakoku_fr_dt")]
        public string? DakokuFrDt { get; private set; }

        [Column("dakoku_fr_tm")]
        public string? DakokuFrTm { get; private set; }

        [Column("dakoku_to_dt")]
        public string? DakokuToDt { get; private set; }

        [Column("dakoku_to_tm")]
        public string? DakokuToTm { get; private set; }

        [Column("kinmu_fr_dt")]
        public string? KinmuFrDt { get; private set; }

        [Column("kinmu_fr_tm")]
        public string? KinmuFrTm { get; private set; }

        [Column("kinmu_to_dt")]
        public string? KinmuToDt { get; private set; }

        [Column("kinmu_to_tm")]
        public string? KinmuToTm { get; private set; }

        [Column("shotei")]
        public int? Shotei { get; set; }

        [Column("sorodo")]
        public int? Sorodo { get; set; }

        [Column("kojo")]
        public int? Kojo { get; set; }

        [Column("kyukei")]
        public int? Kyukei { get; set; }

        [Column("hoteinai")]
        public int? Hoteinai { get; set; }

        [Column("hoteigai")]
        public int? Hoteigai { get; set; }

        [Column("shinya")]
        public int? Shinya { get; set; }

        [Column("hoteikyu")]
        public int? Hoteikyu { get; set; }

        [Column("biko")]
        public string? Biko { get; set; }

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
