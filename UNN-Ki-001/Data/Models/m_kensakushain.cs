using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("m_shain", Schema = "public")]
    public class m_kensakushain
    {
        [Key]
        [Column("shain_no")]
        public string? shain_no { get; set; }
        [Column("name_mei")]
        public string? name_mei { get; set; }
        [Column("shozoku_cd")]
        public string? shozoku_cd { get; set; }
        [Column("shokushu_cd")]
        public string? shokushu_cd { get; set; }
        [Column("koyokeitai_cd")]
        public string? koyokeitai_cd { get; set; }

    }
}
