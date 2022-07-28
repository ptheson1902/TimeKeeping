using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace UNN_Ki_001.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = "Admin")]
    public partial class UserMasterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserMasterModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityUser> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        public IEnumerable<Claim> GetClaims()
        {
            
            return User.Claims;
        }

        public void OnGet()
        {

        }
    }
}
