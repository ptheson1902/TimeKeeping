using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UNN_Ki_001.Data.Models
{
    [Table("AspNetUsers", Schema = "public")]
    public class Users
    {
        [Key]
        [Column("Id")]
        public string id { get; set; }
        [Column("UserName")]
        public string username { get; set; }

    }
}
