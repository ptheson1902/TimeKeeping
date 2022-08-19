using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : BasePageModel
    {
        public List<Display> Data = new List<Display>();

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // セッションからターゲットのリストを取得する
            List<string>? sessionList = HttpContext.Session.GetObject<List<string>>("target");
            List<string> tgtList = new List<string>();

            if(sessionList == null || sessionList.Count == 0)
            {
                var shain = GetCurrentUserShainAsync().Result;
                if(shain == null)
                {

                }
                //tgtList.Add()
            } else
            {
                tgtList.AddRange(sessionList);
            }

            return RedirectToPage("/Attendance/Record/Search");
        }

        public void OnPost()
        {

        }
    }
}
