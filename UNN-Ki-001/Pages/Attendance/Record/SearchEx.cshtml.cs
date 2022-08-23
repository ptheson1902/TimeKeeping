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
    public class SearchEx : BasePageModel
    {
        public SearchEx(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }


        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<M_Shain> _targetList;

        public void OnGet()
        {
            // 一般権限の場合、自身のみを追加して勤務表へ
            if (User.IsInRole("Rookie"))
            {
                
            }
        }
        public IActionResult OnPost(string command, int index)
        {
            if (command != null && command.Equals("sub"))
            {
                // ここまだ

                // セッションに格納して勤務表ページへ飛ぶ
                HttpContext.Session.SetObj(Constants.RECORD_SEARCH_LIST, _targetList);
                HttpContext.Session.SetInt32(Constants.RECORD_SEARCH_CURRENT_INDEX, index);
                return GotoRecord();
            }

            // セレクト
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
                .ToList();
            return Page();
        }

        public IActionResult GotoRecord()
        {
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
