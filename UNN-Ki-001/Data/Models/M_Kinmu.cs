using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_kinmu", Schema = "public")]
    public class M_Kinmu : Reloadable
    {
        public M_Kinmu(string KigyoCd, string KinmuCd , string KinmuNm , string KinmuBunrui/*, string KinmuFrKbn , string KinmuToKbn*/,
            string KinmuFrTm/*, string KinmuToTm ,string Kyukei1FrKbn,string Kyukei1ToKbn, string Kyukei1FrTm , string Kyukei1ToTm ,
            string Kyukei2FrKbn, string Kyukei2ToKbn, string Kyukei2FrTm, string Kyukei2ToTm,
            string Kyukei3FrKbn, string Kyukei3ToKbn, string Kyukei3FrTm, string Kyukei3ToTm,
            string KyukeiAutoFlg , string KinmuFrCtrlFlg , string KinmuFrMarumeKbn , int KinmuFrMarumeTm ,
            string KinmuToMarumeKbn , int KinmuToMarumeTm , string KyukeiFrMarumeKbn ,
            int KyukeiFrMarumeTm, string KyukeiToMarumeKbn , int KyukeiToMarumeTm*/, string ValidFlg)
        {
            this.KigyoCd = KigyoCd;
            this.KinmuCd = KinmuCd;
            this.KinmuNm = KinmuNm;
            this.KinmuBunrui = KinmuBunrui;
            this.KinmuFrTm = KinmuFrTm;
            this.ValidFlg = ValidFlg;
            this.KinmuFrTm = KinmuFrTm;
            this.KinmuToTm = KinmuToTm;
            this.Kyukei1FrTm = Kyukei1FrTm;
            this.Kyukei1ToTm = Kyukei1ToTm;
            this.KyukeiAutoFlg = KyukeiAutoFlg;
            this.Kyukei1FrKbn = Kyukei1FrKbn;
            this.Kyukei1ToKbn = Kyukei1ToKbn;
            this.Kyukei1ToKbn = Kyukei1ToKbn;
            this.KinmuFrKbn = KinmuFrKbn;
            this.Kyukei1FrTm = Kyukei1FrTm;
            this.KinmuToKbn = KinmuToKbn;
            this.Kyukei2FrKbn = Kyukei2FrKbn;
            this.Kyukei3FrKbn = Kyukei3FrKbn;
            this.Kyukei3ToKbn = Kyukei3ToKbn;
            this.Kyukei2ToKbn = Kyukei2ToKbn;
            this.Kyukei2FrTm = Kyukei2FrTm;
            this.Kyukei2FrTm = Kyukei2FrTm;
            this.Kyukei2ToTm = Kyukei2ToTm;
            this.Kyukei3FrTm = Kyukei3FrTm;
            this.Kyukei3ToTm = Kyukei3ToTm;
            this.KinmuToMarumeKbn = KinmuToMarumeKbn;
            this.KinmuFrCtrlFlg = KinmuFrCtrlFlg;
            this.KinmuFrMarumeKbn = KinmuFrMarumeKbn;          
            this.KyukeiFrMarumeKbn = KyukeiFrMarumeKbn;
            this.KinmuFrMarumeTm = KinmuFrMarumeTm;
            this.KinmuToMarumeTm = KinmuToMarumeTm;
            this.KyukeiFrMarumeTm = KyukeiFrMarumeTm;
            this.KyukeiToMarumeKbn = KyukeiToMarumeKbn;
            this.KyukeiToMarumeTm = KyukeiToMarumeTm;
        }



        protected override void Reload(KintaiDbContext context)
        {
            if(ShoteiTm == null)
                ShoteiTm = getShotei();
        }

        private int getShotei()
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
            return (int)stm.TotalMinutes;
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
        [Column("kinmu_fr_kbn")]
        public string? KinmuFrKbn { get; set; }

        [Column("kinmu_to_tm")]
        public string? KinmuToTm { get; set; }
        [Column("kinmu_to_kbn")]
        public string? KinmuToKbn { get; set; }

        [Column("kyukei1_fr_tm")]
        public string? Kyukei1FrTm { get; set; }
        [Column("kyukei1_fr_kbn")]
        public string? Kyukei1FrKbn { get; set; }

        [Column("kyukei1_to_tm")]
        public string? Kyukei1ToTm { get; set; }
        [Column("kyukei1_to_kbn")]
        public string? Kyukei1ToKbn { get; set; }

        [Column("kyukei2_fr_tm")]
        public string? Kyukei2FrTm { get; set; }
        [Column("kyukei2_fr_kbn")]
        public string? Kyukei2FrKbn { get; set; }

        [Column("kyukei2_to_tm")]
        public string? Kyukei2ToTm { get; set; }
        [Column("kyukei2_to_kbn")]
        public string? Kyukei2ToKbn { get; set; }

        [Column("kyukei3_fr_tm")]
        public string? Kyukei3FrTm { get; set; }
        [Column("kyukei3_fr_kbn")]
        public string? Kyukei3FrKbn { get; set; }

        [Column("kyukei3_to_tm")]
        public string? Kyukei3ToTm { get; set; }
        [Column("kyukei3_to_kbn")]
        public string? Kyukei3ToKbn { get; set; }

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

        // ナビゲーションプロパティ
        public List<T_Kinmu> TKinmus { get; set; } = new List<T_Kinmu>();
    }
}
