using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace UNN_Ki_001.Areas.Identity.Pages.Account.Manage
{
    public class UserMasterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserMasterModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<IdentityUser> getUsers()
        {
            return _userManager.Users.OrderBy(user => user.UserName);
        }

        public void OnGet()
        {
            DataTable dt = new DataTable(@"Table");
            dt.Columns.Add("UserName");
            dt.Columns.Add("UserId");

            foreach(var user in getUsers())
            {
                dt.Rows.Add(new object[] { user.UserName, user.Id });
            }
        }
    }
}
