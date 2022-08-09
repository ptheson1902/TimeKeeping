using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_shozoku", Schema = "public")]
    public class shozokukensaku
    {
        [Key]
        [Column("shozoku_cd")]
        public string? shozoku_cd { get; set; }
        [Column("shozoku_nm")]
        public string? shozoku_nm { get; set; }
        [Column("valid_flg")]
        public string? valid_flg { get; set; }


    }
}
