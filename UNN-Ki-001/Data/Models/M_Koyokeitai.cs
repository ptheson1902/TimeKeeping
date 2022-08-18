using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_koyokeitai", Schema = "public")]
    public class M_Koyokeitai
    {
        [Key]
        [Column("koyokeitai_cd")]
        public string? KoyokeitaiCd { get; set; }
        [Column("koyokeitai_nm")]
        public string? KoyokeitaiNm { get; set; }


    }
}
