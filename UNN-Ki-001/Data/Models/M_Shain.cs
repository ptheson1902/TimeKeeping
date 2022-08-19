using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_shain", Schema = "public")]
    public class M_Shain
    {
        public M_Shain(string kigyoCd, string shainNo)
        {
            KigyoCd = kigyoCd;
            ShainNo = shainNo;
        }

        [Key]
        [Column("kigyo_cd")]
        public string KigyoCd { get;  set; }

        [Key]
        [Column("shain_no")]
        public string ShainNo { get; set; }

        [Column("name_sei")]
        public string? NameSei { get; set; }

        [Column("name_mei")]
        public string? NameMei { get; set; }

        [Column("name_kana_sei")]
        public string? NameKanaSei { get; set; }

        [Column("name_kana_mei")]
        public string? NameKanaMei { get; set; }

        [Column("nyusha_dt")]
        public string? NyusyaDt { get; set; }

        [Column("taishoku_dt")]
        public string? TaishokuDt { get; set; }

        [Column("shozoku_cd")]
        public string? ShozokuCd { get; set; }

        [Column("shokushu_cd")]
        public string? ShokushuCd { get; set; }

        [Column("koyokeitai_cd")]
        public string? KoyokeitaiCd { get; set; }
    }
}
