using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kyukei", Schema = "public")]
    public class T_Kyukei
    {
        public T_Kyukei(string? kigyoCd, string? shainNo, string? kinmuDt, int? seqNo, string? dakokuFrKbn, string? dakokuFrDt, string? dakokuFrTm, string? dakokuToKbn, string? dakokuToDt, string? dakokuToTm)
        {
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
            KinmuDt = kinmuDt;
            SeqNo = seqNo == null ? 1 : seqNo;
            DakokuFrKbn = dakokuFrKbn == null ? "0" : dakokuFrKbn;
            DakokuFrDt = dakokuFrDt;
            DakokuFrTm = dakokuFrTm;
            DakokuToKbn = dakokuToKbn == null ? "0" : dakokuToKbn;
            DakokuToDt = dakokuToDt;
            DakokuToTm = dakokuToTm;
        }
        public void kyukeiCal()
        {
            DateTime dftm = DateTime.ParseExact(DakokuFrDt + DakokuFrTm, "yyyyMMddHHmm", null);
            DateTime dttm = DateTime.ParseExact(DakokuToDt + DakokuToTm, "yyyyMMddHHmm", null);
            Kyukei = (int)(dttm - dftm).TotalMinutes;
        }

        [Key]
        [Column("kigyo_cd")]
        public string? KigyoCd { get; set; }
        [Key]
        [Column("shain_no")]
        public string? ShainNo { get; set; }
        [Key]
        [Column("kinmu_dt")]
        public string? KinmuDt { get; set; }
        [Key]
        [Column("seq_no")]
        public int? SeqNo { get; set; }
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
        [Column("kyukei")]
        public int? Kyukei { get; set; }
        [Column("create_dt")]
        public DateTime? CreateDt { get; set; }
        [Column("create_usr")]
        public string? CreateUSR { get; set; }
        [Column("create_pgm")]
        public string? CreatePGM { get; set; }
        [Column("update_dt")]
        public DateTime? UpdateDt { get; set; }
        [Column("update_usr")]
        public string? UpdateUSR { get; set; }
        [Column("update_pgm")]
        public string? UpdatePGM { get; set; }

    }
}
