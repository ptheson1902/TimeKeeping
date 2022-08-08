using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;
using PagedList;

namespace UNN_Ki_001.Pages.VariousMaster
{
    [AllowAnonymous]
    public class shainSearchModel : PageModel
    {
        private readonly UNN_Ki_001.Data.ApplicationDbContext _context;
        public List<Display> Data = new List<Display>();
        
        public shainSearchModel(UNN_Ki_001.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        { 
        }

        public async Task OnPostAsync()
        {
            // フォームからValueを受け取る。
            string shain_no = Request.Form["shain_no"];
            string name_mei = Request.Form["name_mei"];
            string shozoku_nm = Request.Form["shozoku_cd"];
            string shokushu_nm = Request.Form["shokushu_cd"];
            string koyokeitai_nm = Request.Form["koyokeitai_cd"];
            // 所属コード、所属コード、雇用形態コードでJOIN
            var no = from a in _context.shain
                     join b in _context.shozoku
                     on a.shozoku_cd equals b.shozoku_cd
                     join c in _context.shokushu
                     on a.shokushu_cd equals c.shokushu_cd
                     join d in _context.koyokeitai
                     on a.koyokeitai_cd equals d.koyokeitai_cd
                     select new {a.shain_no , a.name_mei , b.shozoku_nm , c.shokushu_nm , d.koyokeitai_nm};

            // 条件による検索すること(value＝nullは検索条件にならないこと。)
            if (!string.IsNullOrEmpty(shain_no))
            {
                no = no.Where(e => e.shain_no.Equals(shain_no) );
            }

           if (!string.IsNullOrEmpty(name_mei))
            {
                no = no.Where(e => e.name_mei.Equals(name_mei));
            }

            if (!string.IsNullOrEmpty(shozoku_nm))
            {
                no = no.Where(e => e.shozoku_nm.Equals(shozoku_nm) );
            }

            if (!string.IsNullOrEmpty(shokushu_nm))
            {
                no = no.Where(e => e.shokushu_nm.Equals(shokushu_nm) );
            }

            if (!string.IsNullOrEmpty(koyokeitai_nm))
            {
                no = no.Where(e => e.koyokeitai_nm.Equals(koyokeitai_nm));
            }


            foreach (var item in no)
            {
                Display d = new Display();
                d.shain_no = item.shain_no;
                d.name_mei = item.name_mei;
                d.shozoku_nm = item.shozoku_nm;
                d.shokushu_nm = item.shokushu_nm;
                d.koyokeitai_nm = item.koyokeitai_nm;

                Data.Add(d);
            }
        }
    }
}
