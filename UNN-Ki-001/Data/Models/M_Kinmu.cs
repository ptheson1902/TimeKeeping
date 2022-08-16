using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_kinmu", Schema = "public")]
    public class M_Kinmu : Reloadable
    {
        public M_Kinmu(string KigyoCd, string KinmuCd)
        {
            this.KigyoCd = KigyoCd;
            this.KinmuCd = KinmuCd;
        }

        public void reload()
        {

            TimeSpan kftm = TimeSpan.FromMinutes(int.Parse(KinmuFrTm == null ? "0" : KinmuFrTm));
            TimeSpan kttm = TimeSpan.FromMinutes(int.Parse(KinmuToTm == null ? "0" : KinmuToTm));
            TimeSpan kkftm1 = TimeSpan.FromMinutes(int.Parse(Kyukei1FrTm == null ? "0" : Kyukei1FrTm));
            TimeSpan kkttm1 = TimeSpan.FromMinutes(int.Parse(Kyukei1ToTm == null ? "0" : Kyukei1ToTm));
            TimeSpan kkftm2 = TimeSpan.FromMinutes(int.Parse(Kyukei2FrTm == null ? "0" : Kyukei2FrTm));
            TimeSpan kkttm2 = TimeSpan.FromMinutes(int.Parse(Kyukei2ToTm == null ? "0" : Kyukei2ToTm));
            TimeSpan kkftm3 = TimeSpan.FromMinutes(int.Parse(Kyukei3FrTm == null ? "0" : Kyukei3FrTm));
            TimeSpan kkttm3 = TimeSpan.FromMinutes(int.Parse(Kyukei3ToTm == null ? "0" : Kyukei3ToTm));

            TimeSpan stm = (kftm - kttm - (kkftm1 - kkttm1) - (kkftm2 - kkttm2) - (kkftm3 - kkttm3));
            ShoteiTm = (int)stm.TotalMinutes;
            
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

        [Column("kinmu_fr_tm")]
        public string? KinmuFrTm { get; set; }

        [Column("kinmu_to_tm")]
        public string? KinmuToTm { get; set; }

        [Column("kyukei1_fr_tm")]
        public string? Kyukei1FrTm { get; set; }


        [Column("kyukei1_to_tm")]
        public string? Kyukei1ToTm { get; set; }

        [Column("kyukei2_fr_tm")]
        public string? Kyukei2FrTm { get; set; }

        [Column("kyukei2_to_tm")]
        public string? Kyukei2ToTm { get; set; }

        [Column("kyukei3_fr_tm")]
        public string? Kyukei3FrTm { get; set; }

        [Column("kyukei3_to_tm")]
        public string? Kyukei3ToTm { get; set; }

        [Column("kyukei_auto_flg")]
        public string? KyukeiAutoFlg { get; set; }

        [Column("shotei_tm")]
        public int? ShoteiTm { get; private set; }

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
        public string? CreateDt { get; }

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
