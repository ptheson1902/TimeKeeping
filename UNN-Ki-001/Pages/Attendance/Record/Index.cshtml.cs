using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
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

        public void OnGet(string? id)
        {
            ShainNo = id;
            var no = from a in _context.m_shains
                     join b in _context.m_shozokus
                     on a.ShozokuCd equals b.ShozokuCd
                     join c in _context.m_shokushus
                     on a.ShokushuCd equals c.ShokushuCd
                     join d in _context.m_koyokeitais
                     on a.KoyokeitaiCd equals d.KoyokeitaiCd
                     where a.ShainNo == id
                     orderby a.ShainNo
                     select new { a.ShainNo, a.NameMei, b.ShozokuNm, c.ShokushuNm, d.KoyokeitaiNm , a.KigyoCd};
            foreach (var item in no)
            {
                ShainName = item.NameMei;
                ShokushuName = item.ShokushuNm;
                ShozokuName = item.ShozokuNm;
                KoyokeitaiName = item.KoyokeitaiNm;
                Kigyo_cd = item.KigyoCd;
            }
        }
    }
}
