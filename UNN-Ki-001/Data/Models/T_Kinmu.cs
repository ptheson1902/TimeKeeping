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
            M_Kinmu? m_Kinmu = context.m_kinmus
                .Where(e => e.KinmuCd.Equals(KinmuCd) && e.KigyoCd.Equals(KigyoCd))
                .FirstOrDefault();

            // 実績開始時間の計算
            if(KinmuFrDate == null)
                KinmuFrWrite(m_Kinmu, context);

            // 実績終了時間の計算
            if (KinmuToDate == null)
                KinmuToWrite(m_Kinmu, context);

            
            if(KinmuToDate != null)
            {
                var test = ((DateTime)KinmuToDate).ToLocalTime();
                Debug.WriteLine(test);
            }
            
            
            // 所定時間の計算
            if(Shotei == null)
                ShoteiWrite(m_Kinmu);

            // 休憩時間の計算
            if (Kyukei == null && KinmuFrDate != null && KinmuToDate != null)
                KyukeiWrite(m_Kinmu, context);
            
            
            // TODO: 総労働時間の計算
            // TODO: 控除時間の計算
        }

        private void KyukeiWrite(M_Kinmu? masterRecord, KintaiDbContext context)
        {
            if(KinmuFrDate != null && KinmuToDate != null)
            {
                // 休憩自動追加フラグが有効の場合
                if (masterRecord != null && masterRecord.KyukeiAutoFlg != null && masterRecord.KyukeiAutoFlg.Equals("1"))
                {
                    Debug.WriteLine("休憩を自動追加します(勤務コード:" + KinmuCd + ")");

                    // 既存の休憩を（もしあれば）すべて削除
                    var target = context.t_Kyukeis
                        .Where(e => e.KigyoCd.Equals(KigyoCd) && e.ShainNo.Equals(ShainNo) && e.KinmuDt.Equals(KinmuDt));
                    context.RemoveRange(target);

                    //// 休憩を自動追加処理
                    List<T_Kyukei> list = new List<T_Kyukei>();
                    int count = 0;

                    // - 休憩１
                    if (masterRecord.Kyukei1FrKbn != null && masterRecord.Kyukei1FrTm != null && masterRecord.Kyukei1ToKbn != null && masterRecord.Kyukei1ToTm != null)
                    {
                        DateControl frDc = new DateControl(KinmuDt, masterRecord.Kyukei1FrTm, masterRecord.Kyukei1FrKbn);
                        DateControl toDc = new DateControl(KinmuDt, masterRecord.Kyukei1ToTm, masterRecord.Kyukei1ToKbn);
                        T_Kyukei kyukei = new T_Kyukei(KigyoCd, ShainNo, KinmuDt, ++count);
                        kyukei.DakokuFrDate = frDc.Origin.ToUniversalTime();
                        kyukei.DakokuToDate = toDc.Origin.ToUniversalTime();
                        list.Add(kyukei);
                    }
                    // - 休憩２
                    if (masterRecord.Kyukei2FrKbn != null && masterRecord.Kyukei2FrTm != null && masterRecord.Kyukei2ToKbn != null && masterRecord.Kyukei2ToTm != null)
                    {
                        DateControl frDc = new DateControl(KinmuDt, masterRecord.Kyukei2FrTm, masterRecord.Kyukei2FrKbn);
                        DateControl toDc = new DateControl(KinmuDt, masterRecord.Kyukei2ToTm, masterRecord.Kyukei2ToKbn);
                        T_Kyukei kyukei = new T_Kyukei(KigyoCd, ShainNo, KinmuDt, ++count);
                        kyukei.DakokuFrDate = frDc.Origin.ToUniversalTime();
                        kyukei.DakokuToDate = toDc.Origin.ToUniversalTime();
                        list.Add(kyukei);
                    }
                    // - 休憩３
                    if (masterRecord.Kyukei3FrKbn != null && masterRecord.Kyukei3FrTm != null && masterRecord.Kyukei3ToKbn != null && masterRecord.Kyukei3ToTm != null)
                    {
                        DateControl frDc = new DateControl(KinmuDt, masterRecord.Kyukei3FrTm, masterRecord.Kyukei3FrKbn);
                        DateControl toDc = new DateControl(KinmuDt, masterRecord.Kyukei3ToTm, masterRecord.Kyukei3ToKbn);
                        T_Kyukei kyukei = new T_Kyukei(KigyoCd, ShainNo, KinmuDt, ++count);
                        kyukei.DakokuFrDate = frDc.Origin.ToUniversalTime();
                        kyukei.DakokuToDate = toDc.Origin.ToUniversalTime();
                        list.Add(kyukei);
                    }


                    // 開始時間と終了時間を確認・修正して追加
                    int totalMinutes = 0;
                    foreach (var item in list)
                    {
                        if (item.DakokuFrDate != null && item.DakokuToDate != null)
                        {

                            var kinmuFr = ((DateTime)KinmuFrDate);
                            var kinmuTo = ((DateTime)KinmuToDate).ToLocalTime();
                            var kyukeiFr = ((DateTime)item.DakokuFrDate).ToLocalTime();
                            var kyukeiTo = ((DateTime)item.DakokuToDate).ToLocalTime();
                            Debug.WriteLine(kinmuFr + "\n" + kinmuTo + "\n" + kyukeiFr + "\n" + kyukeiTo);

                            if (kyukeiFr < kinmuFr)
                            {
                                kyukeiFr = kinmuFr;
                            }
                            if (kyukeiTo > kinmuTo)
                            {
                                kyukeiTo = kinmuTo;
                            }
                            if ((kyukeiTo).CompareTo(kyukeiFr) != 0)
                            {
                                context.Add(item);

                                int minutes = (int)(kyukeiTo - kyukeiFr).TotalMinutes;
                                totalMinutes += minutes;

                                item.DakokuToDate = kyukeiTo.ToUniversalTime();
                                item.DakokuFrDate = kyukeiFr.ToUniversalTime();
                            } else
                            {
                                Debug.WriteLine("ほげほげほげ\n" + item.DakokuFrDate + "\n" + item.DakokuToDate);
                            }
                        }
                    }
                    Kyukei = totalMinutes;
                    return;
                }
                Kyukei = 0;
            }
        }

        private void ShoteiWrite(M_Kinmu? masterRecord)
        {
            Shotei = 0;
            // マスターレコードの取り込み
            if(masterRecord != null && masterRecord.ShoteiTm != null)
            {
                Shotei = masterRecord.ShoteiTm;
                return;
            }
        }

        private void KinmuFrWrite(M_Kinmu? masterRecord, KintaiDbContext context)
        {
            // 実績開始時間の計算
            if (DakokuFrDate != null)
            {
                // マスター勤務レコードが存在しない場合
                if (masterRecord == null)
                {
                    KinmuFrDate = DakokuFrDate;
                    return;
                }

                DateTime DakokuFrDateLocal = ((DateTime)DakokuFrDate).ToLocalTime();

                // 丸め処理を行う
                DateTime result = new DateControl(DakokuFrDateLocal).MarumeProcess(masterRecord.KinmuFrMarumeTm, masterRecord.KinmuFrMarumeKbn).Origin;

                // 刻限の適用
                if (masterRecord.KinmuFrTm != null && masterRecord.KinmuFrKbn != null && masterRecord.KinmuFrCtrlFlg != null)
                {
                    DateTime ZissekiFrDate = new DateControl(KinmuDt, masterRecord.KinmuFrTm, masterRecord.KinmuFrKbn).Origin;
                    if (ZissekiFrDate > result)
                    {
                        result = ZissekiFrDate;
                    }
                }

                KinmuFrDate = result.ToUniversalTime();
            }
        }
        private void KinmuToWrite(M_Kinmu? masterRecord, KintaiDbContext context)
        {
            // マスター勤務レコードが存在しない場合
            if (masterRecord == null)
            {
                KinmuToDate = DakokuToDate;
                return;
            }

            if (DakokuToDate != null)
            {
                DateTime DakokuToDateLocal = ((DateTime)DakokuToDate).ToLocalTime();

                // 丸め処理を行う
                KinmuToDate = new DateControl(DakokuToDateLocal).MarumeProcess(masterRecord.KinmuToMarumeTm, masterRecord.KinmuToMarumeKbn).Origin.ToUniversalTime();

                
                // 整合性を確認（不正なら実績開始時間にあわせる)
                if(KinmuFrDate != null && ((DateTime)KinmuFrDate).ToUniversalTime() > KinmuToDate)
                {
                    KinmuToDate = ((DateTime)KinmuFrDate).ToUniversalTime();
                }


            }
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

        public void SetKinmuFrDate(DateTime dateTime)
        {
            KinmuFrDate = dateTime;
            ClearInfo();
        }

        public void SetKinmuToDate(DateTime dateTime)
        {
            KinmuToDate = dateTime;
            ClearInfo();
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
