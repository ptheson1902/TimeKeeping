using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace UNN_Ki_001.Data.Models
{
    public abstract class BasePageModel : PageModel
    {
        protected readonly KintaiDbContext _kintaiDbContext;
        protected readonly UserManager<AppUser> _userManager;

        protected BasePageModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager)
        {
            _kintaiDbContext = kintaiDbContext;
            _userManager = userManager;
        }

        protected async Task<M_Shain?> GetCurrentUserShainAsync()
        {
            if (User == null || User.Identity == null || User.Identity.Name == null)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return null;

            
            
            var shain = _kintaiDbContext.m_shains
                .Where(e => e.KigyoCd != null && e.KigyoCd.Equals(user.Kigyo_cd) && e.ShainNo != null && e.ShainNo.Equals(user.Shain_no))
                .FirstOrDefault();
            return shain;
        }
    }
}
