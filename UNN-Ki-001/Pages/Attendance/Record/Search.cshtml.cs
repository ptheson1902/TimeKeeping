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
        public string? ShozokuCd { get; set; }
        public string? ShokushuCd { get; set; }
        public string? KoyokeitaiCd { get; set; }
        public string? Message { get; set; }
        public List<M_Shozoku> Shozoku { get; set; }
        public List<M_Shokushu> Shokushu { get; set; }
        public List<M_Koyokeitai> Koyokeitai { get; set; }
        public Search(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }


        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<M_Shain> _targetList = new();

        private static readonly string _TEMP_SEARCH_RESULT_LIST = "tempsearchresultlistsearch";

        public IActionResult OnGet()
        {

            // 現在の社員を取得
            var shain = GetCurrentUserShainAsync().Result;

            // 一般権限の場合、自身のみを追加して勤務表へ
            if (!User.IsInRole("Admin"))
            {
                if(shain == null)
                {
                    // 何の権限も持たない場合はIndexページへ
                    return RedirectToPage("/Index");
                }

                // 自身のみが追加されたリストを作成
                _targetList.Add(shain);
                var tempList = CreateRecordList(_targetList);
                // セッションに格納して勤務表ページへ飛ぶ
                return SendToKinmuhyo(tempList, 0);
            }

            GetData(shain);

            return Page();
        }

        public void GetData(M_Shain? shain)
        {
            if(shain != null)
            {
                Shozoku = (from e in _kintaiDbContext.m_shozokus
                           where e.KigyoCd!.Equals(shain!.KigyoCd)
                           select e).ToList();

                Shokushu = (from e in _kintaiDbContext.m_shokushus
                            where e.KigyoCd.Equals(shain!.KigyoCd)
                            select e).ToList();

                Koyokeitai = (from e in _kintaiDbContext.m_koyokeitais
                              where e.KigyoCd!.Equals(shain!.KigyoCd)
                              select e).ToList();
            }
            else
            {
                return;
            }
        }
        public IActionResult OnPost(string command, int index)
        {
            // 現在の社員を取得
            var shain = GetCurrentUserShainAsync().Result;
            GetData(shain);
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

            // 入力の空白文字を削除
            Input.ReplaceAll(" ", "");
            Input.ReplaceAll("　", "");
            ShozokuCd = Input.ShozokuCd;
            ShokushuCd = Input.ShokushuCd;
            KoyokeitaiCd = Input.KoyokeitaiCd;
            
            // 社員とその関連データを一括Select
            _targetList = _kintaiDbContext.m_shains
                .Where(e => e.KigyoCd.Equals(shain.KigyoCd))
                .Include(shain => shain.Shokushu)
                .Include(shain => shain.Shozoku)
                .Include(shain => shain.Koyokeitai)
                .WhereIf(Input.No != null, shain => shain.ShainNo.Equals(Input.No!))
                .WhereIf(Input.Name != null, shain => (shain.NameSei + shain.NameMei).Equals(Input.Name!))
                .WhereIf(Input.KoyokeitaiCd != null, shain =>     // 雇用形態
                    shain.Koyokeitai != null
                    && shain.Koyokeitai.KoyokeitaiCd.Equals(Input.KoyokeitaiCd!))
                .WhereIf(Input.ShozokuCd != null, shain =>        // 所属
                    shain.Shozoku != null
                    && shain.Shozoku.ShozokuCd!.Equals(Input.ShozokuCd!))
                .WhereIf(Input.ShokushuCd != null, shain =>       // 職種
                    shain.Shokushu != null
                    && shain.Shokushu.ShokushuCd.Equals(Input.ShokushuCd!))
                .OrderBy(shain => shain.ShainNo)
                .ToList();
            if (_targetList == null)
                Message = "検索結果がありません。";
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
            [Display(Name = "社員番号")]
            public string? No { get; set; }
            [Display(Name = "氏名")]
            public string? Name { get; set; }
            [Display(Name = "所属")]
            public string? ShozokuCd { get; set; }
            [Display(Name = "職種")]
            public string? ShokushuCd { get; set; }
            [Display(Name = "雇用形態")]
            public string? KoyokeitaiCd { get; set; }
            public void ReplaceAll(string tgt, string val)
            {
                if (No != null)
                    No = No.Replace(tgt, val);
                if (Name != null)
                    Name = Name.Replace(tgt, val);
                if (ShozokuCd != null)
                    ShozokuCd = ShozokuCd.Replace(tgt, val);
                if (KoyokeitaiCd != null)
                    KoyokeitaiCd = KoyokeitaiCd.Replace(tgt, val);
                if (ShokushuCd != null)
                    ShokushuCd = ShokushuCd.Replace(tgt, val);
            }
        }
        
    }
}
