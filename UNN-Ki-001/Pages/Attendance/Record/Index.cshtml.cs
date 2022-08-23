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


            // 企業コードで設定を読み込み
            

            // 開始日を取得

            // 終了日を取得

            // 勤務データを取り込み
            
            // 存在しないデータを埋める

            // Viewに渡す

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

    /// <summary>
    /// 日付を指定するとその月のカレンダーを生成するオブジェクトです。
    /// </summary>
    class Kinmuhyo
    {
        public DateTime StartDate;
        public DateTime EndDate;

        public Kinmuhyo(DateTime date, int shimebi)
        {
            // 締め日に99を指定された場合は、月末を意味するので割り込み処理
            if (shimebi == 99)
            {
                shimebi = 0;
            }

            // 開始日を計算
            var startYear = date.Year;
            var startMonth = date.Month;


            // 締め日を計算
            var endDate = date.AddMonths(1);
            var endYear = endDate.Year;
            var endMonth = endDate.Month;

            // 日付型を作成
            StartDate = new DateTime(startYear, startMonth, shimebi);
            StartDate = StartDate.AddDays(1);
            EndDate = new DateTime(endYear, endMonth, shimebi);

        }
    }
}
