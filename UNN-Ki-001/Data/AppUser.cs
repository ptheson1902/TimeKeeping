using Microsoft.AspNetCore.Identity;
namespace UNN_Ki_001.Data
{
    public class AppUser : IdentityUser
    {
        public string? Kigyo_cd { get; set; }
        public string? Shain_no { get; set; }
    }
    public class AppRole : IdentityRole
    {

    }

}
