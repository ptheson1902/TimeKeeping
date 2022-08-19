using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : PageModel
    {
        private readonly KintaiDbContext _context;
        public List<Display> Data = new List<Display>();
        public IndexModel(KintaiDbContext context)
        {
            _context = context;
        }
        public string? ShainNo { get; set; }
        public string? ShainName { get; set; }
        public string? ShozokuName { get; set; }
        public string? ShokushuName { get; set; }
        public string? KoyokeitaiName { get; set; }
        public string? Kigyo_cd { get; set; }

        public IActionResult OnGet(string? id)
        {
            return RedirectToPage("/Attendance/Record/Search");
            /*
            ShainNo = id;
            var no = from a in _context.m_shains
                     join b in _context.m_shozokus
                     on a.shozoku_cd equals b.ShozokuCd
                     join c in _context.m_shokushus
                     on a.shokushu_cd equals c.ShokushuCd
                     join d in _context.m_koyokeitais
                     on a.koyokeitai_cd equals d.KoyokeitaiCd
                     where a.shain_no == id
                     orderby a.shain_no
                     select new { a.shain_no, a.name_mei, b.ShozokuNm, c.ShokushuNm, d.KoyokeitaiNm , a.kigyo_cd};
            foreach (var item in no)
            {
                ShainName = item.name_mei;
                ShokushuName = item.ShokushuNm;
                ShozokuName = item.ShozokuNm;
                KoyokeitaiName = item.KoyokeitaiNm;
                Kigyo_cd = item.kigyo_cd;
            }*/
        }

        public void OnPost()
        {

        }
    }
}
