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
        
        public M_Shokushu( string shokushu_cd,string shokushu_nm, string valid_flg , string kigyo_cd)
        {
            KigyoCd = kigyo_cd;
            ShokushuCd = shokushu_cd;
            ShokushuNm = shokushu_nm;
            ValidFlg = valid_flg;

        }
        [Key]
        [Column("kigyo_cd")]
        public string KigyoCd { get; set; }
        [Key]
        [Column("shokushu_cd")]
        public string ShokushuCd { get; set; }
        [Column("shokushu_nm")]
        public string? ShokushuNm { get; set; }
        [Column("valid_flg")]
        public string? ValidFlg { get; set; }

        // ナビゲーションプロパティ
        public List<M_Shain> Shains { get; set; } = new List<M_Shain>();
    }

}
