using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kinmu", Schema = "public")]
    public class T_Kinmu
    {
        /// <summary>
        /// 打刻忘れの際の識別文字
        /// </summary>
        private const　string NULL_CHAR = "N/A";

        /// <summary>
        /// KintaiDbContextクラス
        /// </summary>
        private readonly KintaiDbContext _context;

        public T_Kinmu(string kigyoCd, string shainNo, string kinmuDt, KintaiDbContext context)
        {
            // 必須項目を入力
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
            KinmuDt = kinmuDt;
            _context = context;
        }

        private M_Kinmu? mKinmu
        {
            get
            {
                if(mKinmuBack == null)
                    mKinmuBack = _context.m_kinmus
                        .Where(e => e.KigyoCd.Equals(KigyoCd) && e.KinmuCd.Equals(KinmuCd))
                        .FirstOrDefault();
                return mKinmuBack;
            }
        }
        private M_Kinmu? mKinmuBack { get; set; }

        public void Dakokustart()
        {
            // 最新の勤務記録を参照し、退勤済みか確認する。
            T_Kinmu? record = _context.t_kinmus
                .Where(e => e.KigyoCd.Equals(KigyoCd) && e.KinmuDt.Equals(KinmuDt) && e.ShainNo.Equals(ShainNo) && e.DakokuFrTm != null)
                .OrderByDescending(e => e.KinmuDt)
                .FirstOrDefault();
            if (record != null && (record.DakokuToDt == null || record.DakokuToTm == null))
                throw new Exception("出勤打刻を行うには、退勤打刻が必要です。");

            DateTime now = DateTime.UtcNow;
            DakokuStartWriter(now, true);
        }

        public void DakokuStartWriter(DateTime dateTime, Boolean andKinmuStartWrite = false)
        {
            // 打刻記録のフォーマット
            DateControl dc = new DateControl(dateTime);

            // 打刻記録を保存
            DakokuFrDt = dc.Date;
            DakokuFrTm = dc.Time;

            // 勤務記録も保存
            if (andKinmuStartWrite)
            {
                KinmuStartWriter(dc, true);
            }
        }

        public void DakokuEndWriter(DateTime dateTime, Boolean andKinmuEndWrite = false)
        {
            // 打刻記録のフォーマット
            DateControl dc = new DateControl(dateTime);

            // 打刻記録を保存
            DakokuToDt = dc.Date;
            DakokuToTm = dc.Time;

            // 勤務記録も保存
            if (andKinmuEndWrite)
            {
                KinmuEndWriter(dc, true);
            }
        }

        public void KinmuStartWriter(DateTime dateTime, Boolean marumeProcess = false)
        {
            // 実績記録のフォーマット
            DateControl dc = new DateControl(dateTime);
            KinmuStartWriter(dc, marumeProcess);
        }
        private void KinmuStartWriter(DateControl dc, Boolean marumeProcess = false)
        {
            // 丸め処理の実行
            if (marumeProcess)
            {
                dc = (mKinmu == null) ? dc : dc.MarumeProcess(mKinmu.KinmuFrMarumeTm, mKinmu.KinmuFrMarumeKbn);
                if (mKinmu != null && mKinmu.KinmuFrCtrlFlg != null && mKinmu.KinmuFrCtrlFlg.Equals("0") && mKinmu.KinmuFrTm != null && mKinmu.KinmuFrTm != null)
                {
                    DateControl kinmuFrDc = new DateControl(KinmuDt, mKinmu.KinmuFrTm, mKinmu.KinmuFrKbn);
                    if (dc.Origin < kinmuFrDc.Origin)
                        dc = kinmuFrDc;
                }
            }

            // 実績記録を保存
            KinmuFrDt = dc.Date;
            KinmuFrTm = dc.Time;
        }

        public void KinmuEndWriter(DateTime dateTime, Boolean marumeProcess = false)
        {
            // 実績記録のフォーマット
            DateControl dc = new DateControl(dateTime);
            KinmuEndWriter(dc, marumeProcess);
        }
        private void KinmuEndWriter(DateControl dc, Boolean marumeProcess = false)
        {
            // 丸め処理の実行
            if (marumeProcess)
            {
                dc = (mKinmu == null) ? dc : dc.MarumeProcess(mKinmu.KinmuFrMarumeTm, mKinmu.KinmuFrMarumeKbn);
            }

            // 実績記録を保存
            KinmuFrDt = dc.Date;
            KinmuFrTm = dc.Time;
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

        [Column("dakoku_fr_kbn")]
        public string? DakokuFrKbn { get; private set; }

        [Column("dakoku_fr_dt")]
        public string? DakokuFrDt { get; private set; }

        [Column("dakoku_fr_tm")]
        public string? DakokuFrTm { get; private set; }

        [Column("dakoku_to_kbn")]
        public string? DakokuToKbn { get; private set; }

        [Column("dakoku_to_dt")]
        public string? DakokuToDt { get; private set; }

        [Column("dakoku_to_tm")]
        public string? DakokuToTm { get; private set; }

        [Column("kinmu_fr_kbn")]
        public string? KinmuFrKbn { get; private set; }

        [Column("kinmu_fr_dt")]
        public string? KinmuFrDt { get; private set; }

        [Column("kinmu_fr_tm")]
        public string? KinmuFrTm { get; private set; }

        [Column("kinmu_to_kbn")]
        public string? KinmuToKbn { get; private set; }

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
