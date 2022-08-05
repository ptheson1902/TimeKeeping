using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kinmu", Schema = "public")]
    public class T_Kinmu
    {
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
        private M_Kinmu? KinmuData { get; set; }

        public void DakokuStart(DateTime date)
        {
            DateControl dc = new DateControl(date);
            DakokuFrDt = dc.Date;
            DakokuFrTm = dc.Time;

            // 打刻開始区分の判定　例外処理
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
                using (var _context = new KintaiDbContext())
                {
                    _context.m_kinmus
                        .Where(e => e.KigyoCd.Equals(KigyoCd) && e.KinmuCd.Equals(KinmuCd))
                        .FirstOrDefault();
                }
                    
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
        public DateTime? UpdateDt { get; set; }

        [Column("update_usr")]
        public string? UpdateUsr { get; set; }

        [Column("update_pgm")]
        public string? UpdatePgm { get; set; }
    }
}
