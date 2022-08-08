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

        /// <summary>
        /// 勤務レコードに紐づいた基準情報
        /// </summary>
        private M_Kinmu? m_Kinmu => _context.m_kinmus
                        .Where(e => e.KigyoCd.Equals(KigyoCd) && e.KinmuCd.Equals(KinmuCd))
                        .FirstOrDefault();

        /// <summary>
        /// 打刻登録処理。
        /// DakokuFrDt, DakokuFrTm, KinmuFrDt, KinmuFrTmの値を適切に操作する。
        /// 最新の勤務記録が退勤前ならExceptionをスローする。
        /// </summary>
        /// <param name="date"></param>
        /// <exception cref="Exception">最新の勤務記録が退勤前</exception>
        public void DakokuStart(DateTime? date)
        {
            // TODO: 最新の勤務記録を参照し、退勤済みか確認する。
            T_Kinmu? record = _context.t_kinmus
                .Where(e => e.KigyoCd.Equals(KigyoCd) && e.KinmuDt.Equals(KinmuDt) && e.ShainNo.Equals(ShainNo) && e.DakokuFrTm != null)
                .OrderByDescending(e => e.KinmuDt)
                .FirstOrDefault();
            if (record != null && (record.DakokuToDt == null || record.DakokuToTm == null))
                throw new Exception("出勤打刻を行うには、退勤打刻が必要です。");

            // 打刻忘れの処理
            if(date == null)
            {
                DakokuFrDt = NULL_CHAR;
                DakokuFrTm = NULL_CHAR;
                KinmuFrDt = NULL_CHAR;
                KinmuFrTm = NULL_CHAR;

                return;
            }

            // キャストと丸め処理実行
            DateControl dakokuDc = new DateControl((DateTime)date);
            DateControl marumeDc = (m_Kinmu == null) ? dakokuDc : dakokuDc.MarumeProcess(m_Kinmu.KinmuFrMarumeTm, m_Kinmu.KinmuFrMarumeKbn);

            // 打刻記録を保存
            DakokuFrDt = dakokuDc.Date;
            DakokuFrTm = dakokuDc.Time;

            // 刻限の適用
            if (m_Kinmu != null && m_Kinmu.KinmuFrCtrlFlg != null && m_Kinmu.KinmuFrCtrlFlg.Equals("0") && m_Kinmu.KinmuFrTm != null && m_Kinmu.KinmuFrTm != null)
            {
                DateControl kinmuFrDc = new DateControl(KinmuDt, m_Kinmu.KinmuFrTm, m_Kinmu.KinmuFrKbn);
                if (marumeDc.Origin < kinmuFrDc.Origin)
                    marumeDc = kinmuFrDc;
            }

            // 実績記録を保存
            KinmuFrDt = marumeDc.Date;
            KinmuFrTm = marumeDc.Time;
        }

        /// <summary>
        /// 打刻登録処理。
        /// 現在のサーバー時刻で打刻登録を行う。（DBの時刻とは異なります）
        /// DakokuFrDt, DakokuFrTm, KinmuFrDt, KinmuFrTmの値を適切に操作する。
        /// 最新の勤務記録が退勤前ならExceptionをスローする。
        /// </summary>
        /// <param name="date"></param>
        /// <exception cref="Exception">最新の勤務記録が退勤前</exception>
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
