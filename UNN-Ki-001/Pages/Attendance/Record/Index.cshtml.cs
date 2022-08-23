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
        public List<M_Shain> tgtList = new List<M_Shain>();

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // 管理者権限が有れば、セッションから検索対象を読み取りリストに追加する
            if (User.IsInRole("Admin"))
            {
                // 検索対象が存在しなければNotFound
                List<M_Shain>? sesList = HttpContext.Session.GetObject<List<M_Shain>>(Constants.RECORD_SEARCH_LIST);
                if(sesList == null || sesList.Count == 0)
                {
                    return NotFound();
                }

                tgtList.AddRange(sesList);
            }


            return Page();
        }

        public void OnPost()
        {

        }
        /*
        private DateTime GetShimeStartDate()
        {

        }

        private DateTime GetShimeEndDate()
        {

        }
        */
    }
}
