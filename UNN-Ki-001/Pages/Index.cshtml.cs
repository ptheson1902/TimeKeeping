using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages
{
    [AllowAnonymous]
    public class IndexModel : BasePageModel
    {
        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // ログイン前ならログインページへリダイレクト
            if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            // 社員と紐づいていたら打刻ページへリダイレクト
            var currentShain = GetCurrentUserShainAsync().Result;
            if (currentShain != null)
            {
                return RedirectToPage("/Attendance/Index");
            }

            // 管理者権限を持っていれば社員検索画面へリダイレクト
            return AdminDedicatedRedirect("/Attendance/Record/Search");
        }

        [Authorize(Policy = "Admin")]
        private IActionResult AdminDedicatedRedirect(string path)
        {
            return RedirectToPage(path);
        }
        
        public IActionResult OnPost()
        {
            return OnGet();
        }
    }
}