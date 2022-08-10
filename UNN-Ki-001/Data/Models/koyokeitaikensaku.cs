using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_koyokeitai", Schema = "public")]
    public class koyokeitaikensaku
    {
        [Key]
        [Column("koyokeitai_cd")]
        public string? koyokeitai_cd { get; set; }
        [Column("koyokeitai_nm")]
        public string? koyokeitai_nm { get; set; }


    }
}
