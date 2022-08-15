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

        public override void reload(KintaiDbContext context)
        {

            // マスターレコードの取得
            M_Kinmu? m_Kinmu = context.m_kinmus
                .Where(e => e.KinmuCd.Equals(KinmuCd) && e.KigyoCd.Equals(KigyoCd))
                .FirstOrDefault();

            // マスターレコードが存在しない場合 ///////////////////////////////////////////////////////////
            if (m_Kinmu == null)
            {
                // 実績時間の計算
                if (KinmuFrDate == null) KinmuFrDate = DakokuFrDate;
                if (KinmuToDate == null) KinmuToDate = DakokuToDate;
                
                // マスタレコード無しで集計を行う
            }
            // マスターレコードが存在する場合 ///////////////////////////////////////////////////////////
            else
            {
                // 実績開始時間の計算
                KinmuFrWrite(m_Kinmu);

                // 実績終了時間の計算
                KinmuToWrite(m_Kinmu);

                // マスタレコードを下に集計を行う
                ShoteiZikan(m_Kinmu);
            }
        }
        private void ShoteiZikan(M_Kinmu masterRecord)
        {
            string? kinmuFrTm = masterRecord.KinmuFrTm;
            string? kinmuToTm = masterRecord.KinmuToTm;
            if(kinmuFrTm == null || kinmuToTm == null || KinmuFrDate == null || KinmuToDate == null)
            {
                return;
            }

            DateTime tFrDate = ((DateTime)KinmuFrDate).ToUniversalTime();
            DateTime tToDate = ((DateTime)KinmuToDate).ToUniversalTime();
            DateTime mFrDate = new DateControl(KinmuDt, kinmuFrTm,  masterRecord.KinmuFrKbn).Origin.ToUniversalTime();
            DateTime mToDate = new DateControl(KinmuDt, kinmuToTm, masterRecord.KinmuToKbn).Origin.ToUniversalTime();
            if (mFrDate < tFrDate)
                mFrDate = tFrDate;
            if (mToDate > tToDate)
                mToDate = tToDate;


            var res = mToDate - mFrDate;
            Shotei = (int)res.TotalMinutes;
        }

        private void KinmuFrWrite(M_Kinmu masterRecord)
        {
            // 実績開始時間の計算
            if (DakokuFrDate != null && KinmuFrDate == null /*←注意:仕様です。*/)
            {
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
        private void KinmuToWrite(M_Kinmu masterRecord)
        {
            if (DakokuToDate != null && KinmuToDate == null /*←仕様です。*/)
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
        public string? KinmuCd { get; set; }

        [Column("dakoku_fr_date")]
        public DateTime? DakokuFrDate { get; set; }

        [Column("dakoku_to_date")]
        public DateTime? DakokuToDate { get; set; }

        [Column("kinmu_fr_date")]
        public DateTime? KinmuFrDate { get; set; }

        [Column("kinmu_to_date")]
        public DateTime? KinmuToDate { get; set; }

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
