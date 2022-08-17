using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance
{
    public class Kinmuhyo1Model : PageModel
    {
        private readonly KintaiDbContext _context;
        public List<Display> Data = new List<Display>();
        public Kinmuhyo1Model(KintaiDbContext context)
        {
            _context = context;
        }
        public string? ShainNo { get; set; }
        public string? ShainName { get; set; }
        public string? ShozokuName { get; set; }
        public string? ShokushuName { get; set; }
        public string? KoyokeitaiName { get; set; }

        public void OnGet(string? id)
        {
            ShainNo = id;
            var no = from a in _context.shain
                     join b in _context.shozoku
                     on a.shozoku_cd equals b.shozoku_cd
                     join c in _context.shokushu
                     on a.shokushu_cd equals c.shokushu_cd
                     join d in _context.koyokeitai
                     on a.koyokeitai_cd equals d.koyokeitai_cd
                     where a.shain_no == id
                     select new {a.name_mei, b.shozoku_nm, c.shokushu_nm, d.koyokeitai_nm };
            foreach (var item in no)
            {
                ShainName = item.name_mei;
                ShokushuName = item.shokushu_nm;
                ShozokuName = item.shozoku_nm;
                KoyokeitaiName = item.koyokeitai_nm;
            }
        }
    }
}
