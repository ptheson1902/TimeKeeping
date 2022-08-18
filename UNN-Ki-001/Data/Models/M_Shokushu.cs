using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
   
    [Table("m_shokushu", Schema = "public")]
    public class M_Shokushu
    {
        public M_Shokushu()
        {
        }

        public M_Shokushu(string? shokushu_cd, string? shokushu_nm, string? valid_flg, string? kigyo_cd)
        {
            this.ShokushuCd = shokushu_cd;
            this.ShokushuNm = shokushu_nm;
            this.ValidFlg = valid_flg;
            this.KigyoCd = kigyo_cd;
        }
        [Key]
        [Column("shokushu_cd")]
        public string? ShokushuCd { get; set; }
        [Column("shokushu_nm")]
        public string? ShokushuNm { get; set; }
        [Column("valid_flg")]
        public string? ValidFlg { get; set; }
        [Column("kigyo_cd")]
        public string? KigyoCd { get; set; }


    }

}
