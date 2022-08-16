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

            // 実績開始時間の計算
            KinmuFrWrite(m_Kinmu);

            // 実績終了時間の計算
            KinmuToWrite(m_Kinmu);

            // 所定時間の計算
            ShoteiZikan(m_Kinmu);

            // TODO: 休憩時間の計算
            // TODO: 総労働時間の計算
            // TODO: 控除時間の計算
            // TODO: 法廷内時間の計算
            // TODO: 法定外時間の計算
            // TODO: 深夜時間の計算
            // TODO: 法定休日時間の計算
        }
        private void ShoteiZikan(M_Kinmu? masterRecord)
        {
            // ヌルを許容しない
            if(KinmuFrDate == null ||  KinmuToDate == null)
            {
                return;
            }

            // 勤務データの宣言
            DateTime tKinmuFrDate = ((DateTime)KinmuFrDate).ToUniversalTime();
            DateTime tKinmuToDate = ((DateTime)KinmuToDate).ToUniversalTime();

            // マスターレコードの適用
            if(masterRecord != null)
            {
                if(masterRecord.KinmuFrTm != null)
                {

                }
            }

            /*
            string? kinmuFrTm = masterRecord.KinmuFrTm;
            string? kinmuToTm = masterRecord.KinmuToTm;
            if(kinmuFrTm == null || kinmuToTm == null || KinmuFrDate == null || KinmuToDate == null)
            {
                Shotei = masterRecord.ShoteiTm;
                return;
            }

            // 必須データが存在しない場合
            if (KinmuFrDate == null || KinmuToDate == null)
            {
                return;
            }
            var res = (DateTime)KinmuToDate - (DateTime)KinmuFrDate;

            */
            var res = mToDate - mFrDate;
            Shotei = (int)res.TotalMinutes;
        }

        private void KinmuFrWrite(M_Kinmu? masterRecord)
        {
            // マスター勤務レコードが存在しない場合
            if(masterRecord == null)
            {
                KinmuFrDate = DakokuFrDate;
                return;
            }

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
        private void KinmuToWrite(M_Kinmu? masterRecord)
        {
            // マスター勤務レコードが存在しない場合
            if (masterRecord == null)
            {
                KinmuToDate = DakokuToDate;
                return;
            }

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
