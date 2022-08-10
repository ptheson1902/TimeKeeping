using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_shozoku", Schema = "public")]
    public class shozokukensaku
    {
        public shozokukensaku(string? shozoku_cd, string? shozoku_nm, string? valid_flg, string? kigyo_cd)
        {
            this.shozoku_cd = shozoku_cd;
            this.shozoku_nm = shozoku_nm;
            this.valid_flg = valid_flg;
            this.kigyo_cd = kigyo_cd;
        }

        [Key]
        [Column("shozoku_cd")]
        public string? shozoku_cd { get; set; }

        [Column("shozoku_nm")]
        public string? shozoku_nm { get; set; }
        [Column("valid_flg")]
        public string? valid_flg { get; set; }
        [Column("kigyo_cd")]
        public string? kigyo_cd { get; set; }

    }
}
