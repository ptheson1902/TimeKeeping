using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
   
    [Table("m_shokushu", Schema = "public")]
    public class M_Shokushu
    {
        [Key]
        [Column("shokushu_cd")]
        public string? ShokushuCd { get; set; }
        [Column("shokushu_nm")]
        public string? ShokushuNm { get; set; }


    }

}
