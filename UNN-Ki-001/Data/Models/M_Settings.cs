using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_settings", Schema = "public")]
    public class M_Settings
    {
        public M_Settings(string kigyoCd)
        {
            KigyoCd = kigyoCd;
        }

        [Key]
        [Column("kigyo_cd")]
        public string KigyoCd { get; set; }

        [Column("shime_dt")]
        public int? ShimeDt { get; set; }

        /*
        public int CalcShimeDt(int year, int month)
        {
            if (month > 12) throw new Exception();
        }*/
    }
}
