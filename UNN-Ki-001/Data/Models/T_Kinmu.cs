using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("t_kinmu", Schema = "public")]
    public class T_Kinmu
    {
        public T_Kinmu(string kigyo_cd, string kinmu_cd)
        {
            this.kigyo_cd = kigyo_cd;
            this.kinmu_cd = kinmu_cd;
        }

        [Key]
        [Column("kigyo_cd")]
        public string kigyo_cd { get; set; }

        [Key]
        [Column("kinmu_id")]
        public string kinmu_cd { get; set; }

        [Column("kinmu_nm")]
        public string? kinmu_nm { get; set; }

        [Column("kinmu_bunrui")]
        public string? kinmu_bunrui { get; set; }

        [Column("kinmu_fr_kbn")]
        public string? kinmu_fr_kbn { get; set; }

        [Column("kinmu_fr_tm")]
        public string? kinmu_fr_tm  { get; set; }

        [Column("kinmu_to_kbn")]
        public string? kinmu_to_kbn { get; set; }

        [Column("kyukei1_fr_kbn")]
        public string? kyukei1_fr_kbn { get; set; }

        [Column("kyukei1_fr_tm")]
        public string? kyukei1_fr_tm { get; set; }
    }
}
