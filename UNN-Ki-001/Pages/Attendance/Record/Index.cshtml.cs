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
        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // 参照リストをセッションから読み込み
            var temp = HttpContext.Session.GetObject<ShainSearchRecordList>(Constants.SEARCH_RECORD_LIST);

            // 参照対象が確認できなければNotFound
            if(temp == null)
            {
                // 整合性チェック
                var length = temp.List.Count;
                var index = temp.CurrentIndex;
                if(length == 0 || index < 0 || index >= length)
                {
                    return NotFound();
                }
            }

            // 確認が済んだのでNULL非許容型へ再配置します
            ShainSearchRecordList targetList = temp;

            // カレントインデックスの情報を取り出し、ビューに渡します。
            ShainSearchRecord target = targetList.GetCurrent();
            ViewData["target"] = target;

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
