using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
   
    [Table("m_shokushu", Schema = "public")]
    public class M_Shokushu
    {
        [Key]
        [Column("shokushu_cd")]
        public string? shokushu_cd { get; set; }
        [Column("shokushu_nm")]
        public string? shokushu_nm { get; set; }


    }

}
