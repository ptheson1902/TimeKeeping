using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_koyokeitai", Schema = "public")]
    public class M_Koyokeitai
    {
        /*
        public M_Koyokeitai(string kigyo_cd, string koyokeitai_cd)
        {
            KigyoCd = kigyo_cd;
            KoyokeitaiCd = koyokeitai_cd;
        }*/

        [Column("kigyo_cd")]
        public string KigyoCd { get; set; }
        [Column("koyokeitai_cd")]
        public string KoyokeitaiCd { get; set; }
        [Column("koyokeitai_nm")]
        public string? KoyokeitaiNm { get; set; }
        [Column("valid_flg")]
        public string? ValidFlg { get; set; }


        // ナビゲーションプロパティ
        public List<M_Shain> Shains { get; set; } = new List<M_Shain>();
    }
}
