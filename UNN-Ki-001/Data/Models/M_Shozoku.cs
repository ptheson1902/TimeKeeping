using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_shozoku", Schema = "public")]
    public class M_Shozoku
    {
        public M_Shozoku()
        {
        }

        public M_Shozoku(string? shozoku_cd, string? shozoku_nm, string? valid_flg, string? kigyo_cd)
        {
            this.ShozokuCd = shozoku_cd;
            this.ShozokuNm = shozoku_nm;
            this.ValidFlg = valid_flg;
            this.KigyoCd = kigyo_cd;
        }

        [Key]
        [Column("shozoku_cd")]
        public string? ShozokuCd { get; set; }

        [Column("shozoku_nm")]
        public string? ShozokuNm { get; set; }
        [Column("valid_flg")]
        public string? ValidFlg { get; set; }
        [Column("kigyo_cd")]
        public string? KigyoCd { get; set; }

    }
}
