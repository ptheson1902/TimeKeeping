using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UNN_Ki_001.Data.Models
{
    [Table("m_users", Schema = "public")]
    public class Users
    {
        [Key]
        [Column("user_id")]
        public string user_id { get; set; }
        [Column("password")]
        public string password { get; set; }
    }
}
