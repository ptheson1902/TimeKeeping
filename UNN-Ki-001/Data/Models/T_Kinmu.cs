using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kinmu", Schema = "public")]
    public class T_Kinmu
    {
        static private KintaiDbContext _context = new KintaiDbContext();

        public T_Kinmu(string kigyoCd, string shainNo, string kinmuDt)
        {
            // 必須項目を入力
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
            KinmuDt = kinmuDt;
        }

        /// <summary>
        /// 勤務レコードに紐づいた基準情報
        /// </summary>
        private M_Kinmu? m_Kinmu { get; set; }

        public void DakokuStart(DateTime? date)
        {
            // 打刻忘れの処理
            if(date == null)
            {
                string NULL_CHAR = "N/A";

                DakokuFrDt = NULL_CHAR;
                DakokuFrTm = NULL_CHAR;
                KinmuFrDt = NULL_CHAR;
                KinmuFrTm = NULL_CHAR;

                return;
            }

            // キャストと丸め処理実行
            DateControl dakokuDc = new DateControl((DateTime)date);
            DateControl marumeDc = (m_Kinmu == null) ? dakokuDc : dakokuDc.MarumeProcess(m_Kinmu.KinmuFrMarumeTm, m_Kinmu.KinmuFrMarumeKbn);

            // TODO: 適切な打刻時間(当日・前日・翌日）で無いなら例外をスローする


            // 打刻記録を保存
            DakokuFrDt = dakokuDc.Date;
            DakokuFrTm = dakokuDc.Time;

            // 実績記録
            KinmuFrDt = marumeDc.Date;
            if (m_Kinmu != null && m_Kinmu.KinmuFrCtrlFlg == null && m_Kinmu.KinmuFrCtrlFlg.Equals("0") && m_Kinmu.KinmuFrTm != null && m_Kinmu.KinmuFrTm != null)
            {
                // 刻限（開始）をDateControl型にキャスト
                DateControl kinmuFrDc = new DateControl(KinmuDt, m_Kinmu.KinmuFrTm, m_Kinmu.KinmuFrKbn);

                // TODO: 実績記録を保存



            } else
            {
                KinmuFrTm = marumeDc.Time;
            }
        }

        public void DakokuStart()
        {
            DakokuStart(DateTime.Now);
        }

        public void DakokuEnd(DateTime date)
        {

        }

        public void DakokuEnd()
        {
            DakokuEnd(DateTime.Now);
        }
        
        public void KinmuStart(DateTime date)
        {

        }

        public void KinmuEnd(DateTime date)
        {

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
        public string? KinmuCd
        {
            get
            {
                return KinmuCd;
            }
            set
            {
                KinmuCd = value;
                m_Kinmu = _context.m_kinmus
                        .Where(e => e.KigyoCd.Equals(KigyoCd) && e.KinmuCd.Equals(KinmuCd))
                        .FirstOrDefault();
            }
        }

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
        public string? UpdateDt { get; set; }

        [Column("update_usr")]
        public DateTime? UpdateUsr { get; set; }

        [Column("update_pgm")]
        public string? UpdatePgm { get; set; }
    }
}
