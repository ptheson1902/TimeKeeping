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
        public List<string> tgtList = new List<string>();

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // 管理者権限が有れば、セッションから検索対象を読み取りリストに追加する
            if (User.IsInRole("Admin"))
            {
                // 検索対象が存在しなければNotFound
                List<string>? sesList = HttpContext.Session.GetObject<List<string>>("target");
                if(sesList == null || sesList.Count == 0)
                {
                    return NotFound();
                }

                tgtList.AddRange(sesList);
            }
            // 一般権限で有れば、自分自身をリストに追加
            else
            {
                var me = GetCurrentUserShainAsync().Result;
                
                // 社員情報と紐づいてなければ、トップページへリダイレクト
                if(me == null)
                {
                    return RedirectToPage("/");
                }

                tgtList.Add(me.ShainNo);
            }


            return Page();
        }

        public void OnPost()
        {

        }
    }
}
