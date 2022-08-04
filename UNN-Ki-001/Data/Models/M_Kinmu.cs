using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_kinmu", Schema = "public")]
    public class M_Kinmu
    {
        public M_Kinmu(string KigyoCd, string KinmuCd)
        {
            this.KigyoCd = KigyoCd;
            this.KinmuCd = KinmuCd;
        }

        [Key]
        [Column("kigyo_cd")]
        public string KigyoCd { get; set; }

        [Key]
        [Column("kinmu_cd")]
        public string KinmuCd { get; set; }

        [Column("kinmu_nm")]
        public string? KinmuNm { get; set; }

        [Column("kinmu_bunrui")]
        public string? KinmuBunrui { get; set; }

        [Column("kinmu_fr_kbn")]
        public string? KinmuFrKbn { get; set; }

        [Column("kinmu_fr_tm")]
        public string? KinmuFrTm { get; set; }

        [Column("kinmu_to_kbn")]
        public string? KinmuToKbn { get; set; }

        [Column("kinmu_to_tm")]
        public string? KinmuToTm { get; set; }

        [Column("kyukei1_fr_kbn")]
        public string? Kyukei1FrKbn { get; set; }

        [Column("kyukei1_fr_tm")]
        public string? Kyukei1FrTm { get; set; }

        [Column("kyukei1_to_kbn")]
        public string? Kyukei1ToKbn { get; set; }

        [Column("kyukei1_to_tm")]
        public string? Kyukei1ToTm { get; set; }

        [Column("kyukei2_fr_kbn")]
        public string? Kyukei2FrKbn { get; set; }

        [Column("kyukei2_fr_tm")]
        public string? Kyukei2FrTm { get; set; }

        [Column("kyukei2_to_kbn")]
        public string? Kyukei2ToKbn { get; set; }

        [Column("kyukei2_to_tm")]
        public string? Kyukei2ToTm { get; set; }

        [Column("kyukei3_fr_kbn")]
        public string? Kyukei3FrKbn { get; set; }

        [Column("kyukei3_fr_tm")]
        public string? Kyukei3FrTm { get; set; }

        [Column("kyukei3_to_kbn")]
        public string? Kyukei3ToKbn { get; set; }

        [Column("kyukei3_to_tm")]
        public string? Kyukei3ToTm { get; set; }

        [Column("kyukei_auto_flg")]
        public string? KyukeiAutoFlg { get; set; }

        [Column("shotei_tm")]
        public string? ShoteiTm { get; set; }

        [Column("kinmu_fr_ctrl_flg")]
        public string? KinmuFrCtrlFlg { get; set; }

        [Column("kinmu_fr_marume_tm")]
        public int? KinmuFrMarumeTm  { get; set; }

        [Column("kinmu_fr_marume_kbn")]
        public string? KinmuFrMarumeKbn { get; set; }

        [Column("kinmu_to_marume_tm")]
        public int? KinmuToMarumeTm { get; set; }

        [Column("kinmu_to_marume_kbn")]
        public string? KinmuToMarumeKbn { get; set; }

        [Column("kyukei_fr_marume_tm")]
        public int? KyukeiFrMarumeTm { get; set; }

        [Column("kyukei_fr_marume_kbn")]
        public string? KyukeiFrMarumeKbn { get; set; }

        [Column("kyukei_to_marume_tm")]
        public int? KyukeiToMarumeTm { get; set; }

        [Column("kyukei_to_marume_kbn")]
        public string? KyukeiToMarumeKbn { get; set; }

        [Column("valid_flg")]
        public string? ValidFlg { get; set; }

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
