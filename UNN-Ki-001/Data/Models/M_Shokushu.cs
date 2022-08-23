using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
   
    [Table("m_shokushu", Schema = "public")]
    public class M_Shokushu
    {
        /*
        public M_Shokushu(string kigyo_cd, string shokushu_cd)
        {
            KigyoCd = kigyo_cd;
            ShokushuCd = shokushu_cd;
        }*/
        [Column("kigyo_cd")]
        public string KigyoCd { get; set; }
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
