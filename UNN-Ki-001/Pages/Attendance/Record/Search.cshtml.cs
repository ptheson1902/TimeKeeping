using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;
namespace UNN_Ki_001.Pages.Attendance.Record
{

    [Authorize(Policy = "Rookie")]
    public class Search : BasePageModel
    {
        public Search(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }


        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<M_Shain> _targetList = new();

        private static readonly string _TEMP_SEARCH_RESULT_LIST = "tempsearchresultlistsearch";

        public IActionResult OnGet()
        {


            // 一般権限の場合、自身のみを追加して勤務表へ
            if (!User.IsInRole("Admin"))
            {
                // 現在の社員を取得
                var me = GetCurrentUserShainAsync().Result;

                if(me == null)
                {
                    // 何の権限も持たない場合はIndexページへ
                    return RedirectToPage("/Index");
                }

                // 自身のみが追加されたリストを作成
                _targetList.Add(me);
                var tempList = CreateRecordList(_targetList);
                // セッションに格納して勤務表ページへ飛ぶ
                return SendToKinmuhyo(tempList, 0);
            }
            return Page();
        }
        public IActionResult OnPost(string command, int index)
        {
            // 行選択時の処理
            if (command != null && command.Equals("sub") && index >= 0)
            {

                // 一時データをセッションから取得
                var getData = HttpContext.Session.GetObject<List<ShainSearchRecord>>(_TEMP_SEARCH_RESULT_LIST);
                // 一時データをセッションから削除
                HttpContext.Session.Remove(_TEMP_SEARCH_RESULT_LIST);
                // 決定データ（とCurrentIndex)をセッションに格納して勤務表ページへ飛ぶ
                return SendToKinmuhyo(getData, index);
            }

            // 以下検索ボタン押下時の処理
            // 社員とその関連データを一括Select
            _targetList = _kintaiDbContext.m_shains
                .Include(shain => shain.Shokushu)
                .Include(shain => shain.Shozoku)
                .Include(shain => shain.Koyokeitai)
                .WhereIf(Input.No != null, shain => shain.ShainNo.Contains(Input.No!))
                .WhereIf(Input.Name != null, shain => (shain.NameSei + shain.NameMei).Contains(Input.Name!))
                .WhereIf(Input.KigyoCd != null, shain => shain.KigyoCd.Contains(Input.KigyoCd!))
                .WhereIf(Input.KoyokeitaiName != null, shain =>     // 雇用形態
                    shain.Koyokeitai != null
                    && shain.Koyokeitai.KoyokeitaiNm != null
                    && shain.Koyokeitai.KoyokeitaiNm.Contains(Input.KoyokeitaiName!))
                .WhereIf(Input.ShozokuName != null, shain =>        // 所属
                    shain.Shozoku != null
                    && shain.Shozoku.ShozokuNm != null
                    && shain.Shozoku.ShozokuNm.Contains(Input.ShozokuName!))
                .WhereIf(Input.ShokushuName != null, shain =>       // 職種
                    shain.Shokushu != null
                    && shain.Shokushu.ShokushuNm != null
                    && shain.Shokushu.ShokushuNm.Contains(Input.ShokushuName!))
                .OrderBy(shain => shain.ShainNo)
                .ToList();

            // 検索結果をシリアライズ可能なListにしてセッションに一時データとして格納
            var tempList = CreateRecordList(_targetList);
            HttpContext.Session.SetObj(_TEMP_SEARCH_RESULT_LIST, tempList);


            return Page();
        }

        private List<ShainSearchRecord> CreateRecordList(List<M_Shain> shainList)
        {
            List<ShainSearchRecord> tempList = new();
            foreach (var item in shainList)
            {
                ShainSearchRecord temp = new();
                temp.KigyoCd = item.KigyoCd;
                temp.ShainNo = item.ShainNo;
                temp.ShainNm = (item.NameSei + item.NameMei).Replace(" ", "");
                temp.ShokushuCd = item.ShokushuCd;
                if (item.Shokushu != null)
                {
                    temp.ShokushuNm = item.Shokushu.ShokushuNm;
                }
                temp.ShozokuCd = item.ShozokuCd;
                if (item.Shozoku != null)
                {
                    temp.ShozokuNm = item.Shozoku.ShozokuNm;
                }
                temp.KoyokeitaiCd = item.KoyokeitaiCd;
                if (item.Koyokeitai != null)
                {
                    temp.KoyokeitaiNm = item.Koyokeitai.KoyokeitaiNm;
                }
                tempList.Add(temp);
            }
            return tempList;
        }

        public IActionResult SendToKinmuhyo(List<ShainSearchRecord>? list, int index)
        {
            if(list != null)
            {
                // 決定データをセッションに格納して勤務表ページへ飛ぶ
                ShainSearchRecordList resultList = new ShainSearchRecordList(list, index);
                HttpContext.Session.SetObj(Constants.SEARCH_RECORD_LIST, resultList);
            }

            return RedirectToPage("/Attendance/Record/Index");
        }


        public class InputModel
        {
            [Display(Name = "企業コード")]
            public string? KigyoCd { get; set; }
            [Display(Name = "社員番号")]
            public string? No { get; set; }
            [Display(Name = "氏名")]
            public string? Name { get; set; }
            [Display(Name = "所属")]
            public string? ShozokuName { get; set; }
            [Display(Name = "職種")]
            public string? ShokushuName { get; set; }
            [Display(Name = "雇用形態")]
            public string? KoyokeitaiName { get; set; }
        }
        
    }
}
