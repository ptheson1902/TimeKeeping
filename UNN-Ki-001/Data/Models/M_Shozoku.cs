using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_shozoku", Schema = "public")]
    public class M_Shozoku
    {/*
        public M_Shozoku(string kigyo_cd, string shozoku_cd)
        {
            ShozokuCd = shozoku_cd;
            KigyoCd = kigyo_cd;
        }*/

        [Column("kigyo_cd")]
        public string? KigyoCd { get; set; }
        [Column("shozoku_cd")]
        public string? ShozokuCd { get; set; }

        [Column("shozoku_nm")]
        public string? ShozokuNm { get; set; }
        [Column("valid_flg")]
        public string? ValidFlg { get; set; }

        // ナビゲーションプロパティ
        public List<M_Shain> Shains { get; set; } = new List<M_Shain>();
    }
}
