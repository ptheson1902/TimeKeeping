using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kinmu", Schema = "public")]
    public class T_Kinmu : Reloadable
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

        public void reload()
        {
            // TODO: 所定時間の計算
            // TODO: 総労働時間の計算
            // TODO: 法廷内時間の計算
            // TODO: 法定外時間の計算
            // TODO: 深夜時間の計算
            // TODO: 法定休日（労働）時間の計算
            // TODO: 休憩時間の計算
            // TODO: 控除時間の計算
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

        public DateControl? DakokuFrDc
        {
            get
            {
                if(DakokuFrDc == null && DakokuFrDt != null && DakokuFrTm != null)
                {
                    DakokuFrDc = new DateControl(DakokuFrDt, DakokuFrTm);
                }
                return DakokuFrDc;
            }
            set
            {
                DakokuFrDc = value;
                if(value != null)
                {
                    DakokuFrDt = value.Date;
                    DakokuFrTm = value.Time;
                }
            }
        }
        public DateControl? DakokuToDc
        {
            get
            {
                if (DakokuToDc == null && DakokuToDt != null && DakokuToTm != null)
                {
                    DakokuToDc = new DateControl(DakokuToDt, DakokuToTm);
                }
                return DakokuToDc;
            }
            set
            {
                DakokuToDc = value;
                if (value != null)
                {
                    DakokuToDt = value.Date;
                    DakokuToTm = value.Time;
                }
            }
        }
        public DateControl? KinmuFrDc
        {
            get
            {
                if (KinmuFrDc == null && KinmuFrDt != null && KinmuFrTm != null)
                {
                    KinmuFrDc = new DateControl(KinmuFrDt, KinmuFrTm);
                }
                return KinmuFrDc;
            }
            set
            {
                KinmuFrDc = value;
                if (value != null)
                {
                    KinmuFrDt = value.Date;
                    KinmuFrTm = value.Time;
                }
            }
        }
        public DateControl? KinmuToDc
        {
            get
            {
                if (KinmuToDc == null && KinmuToDt != null && KinmuToTm != null)
                {
                    KinmuToDc = new DateControl(KinmuToDt, KinmuToTm);
                }
                return KinmuToDc;
            }
            set
            {
                KinmuToDc = value;
                if (value != null)
                {
                    KinmuToDt = value.Date;
                    KinmuToTm = value.Time;
                }
            }
        }

        public void DakokuStart()
        {
            // 最新の勤務記録を参照し、退勤済みか確認する。
            T_Kinmu? record = _context.t_kinmus
                .Where(e => e.KigyoCd.Equals(KigyoCd) && e.KinmuDt.Equals(KinmuDt) && e.ShainNo.Equals(ShainNo) && e.DakokuFrTm != null)
                .OrderByDescending(e => e.KinmuDt)
                .FirstOrDefault();
            if (record != null && (record.DakokuToDt == null || record.DakokuToTm == null))
                throw new Exception("出勤打刻を行うには、退勤打刻が必要です。");

            DateTime now = DateTime.Now;
            DakokuStartWriter(now, true);
        }

        public void DakokuEnd()
        {
            // 出勤済みか確認する。
            if (DakokuFrDt == null || DakokuFrTm == null)
                throw new Exception("退勤打刻を行うには、先に出勤打刻が必要です。");

            DateTime now = DateTime.Now;
            DakokuEndWriter(now, true);
        }

        public void DakokuStartWriter(DateTime dateTime, Boolean andKinmuStartWrite = false)
        {
            // 打刻記録のフォーマット
            DateControl dc = new DateControl(dateTime);

            // 打刻記録を保存
            DakokuFrDc = dc;

            // 勤務記録も保存
            if (andKinmuStartWrite)
            {
                KinmuStartWriter(DakokuFrDc, true);
            }
        }

        public void DakokuEndWriter(DateTime dateTime, Boolean andKinmuEndWrite = false)
        {
            // 打刻記録のフォーマット
            DateControl dc = new DateControl(dateTime);

            // 打刻記録を保存
            DakokuToDc = dc;

            // 勤務記録も保存
            if (andKinmuEndWrite)
            {
                KinmuEndWriter(DakokuToDc, true);
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
                    DateControl kinmuFrDc = new DateControl(KinmuDt, mKinmu.KinmuFrTm);
                    if (dc.Origin < kinmuFrDc.Origin)
                        dc = kinmuFrDc;
                }
            }

            // 実績記録を保存
            KinmuFrDc = dc;
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
            KinmuToDc = dc;
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


        [Column("dakoku_fr_dt")]
        public string? DakokuFrDt { get; set; }

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
