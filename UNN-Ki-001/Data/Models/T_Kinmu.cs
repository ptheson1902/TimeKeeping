using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kinmu", Schema = "public")]
    public class T_Kinmu
    {
        public T_Kinmu(string kigyoCd, string shainNo, string kinmuDt)
        {
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
            KinmuDt = kinmuDt;
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
        public string? DakokuFrKbn { get; set; }

        [Column("dakoku_fr_dt")]
        public string? DakokuFrDt { get; set; }

        [Column("dakoku_fr_tm")]
        public string? DakokuFrTm { get; set; }

        [Column("dakoku_to_kbn")]
        public string? DakokuToKbn { get; set; }

        [Column("dakoku_to_dt")]
        public string? DakokuToDt { get; set; }

        [Column("dakoku_to_tm")]
        public string? DakokuToTm { get; set; }

        [Column("kinmu_fr_kbn")]
        public string? KinmuFrKbn { get; set; }

        [Column("kinmu_fr_dt")]
        public string? KinmuFrDt { get; set; }

        [Column("kinmu_fr_tm")]
        public string? KinmuFrTm { get; set; }

        [Column("kinmu_to_kbn")]
        public string? KinmuToKbn { get; set; }

        [Column("kinmu_to_dt")]
        public string? KinmuToDt { get; set; }

        [Column("kinmu_to_tm")]
        public string? KinmuToTm { get; set; }

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
