using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;
namespace UNN_Ki_001.Pages.VariousMaster
{
    [AllowAnonymous]
    public class ShainSearchModel : PageModel
    {
        private readonly KintaiDbContext _context;
        private readonly ApplicationDbContext context1;
        public List<Display> Data = new List<Display>();
        public Display? Data1;
        public string Shain_no { get; set; }
        public string Name_mei { get; set; }
        public string Shozoku_nm { get; set; }
        public string Shokushu_nm { get; set; }
        public string Koyokeitai_nm { get; set; }

        public ShainSearchModel(UNN_Ki_001.Data.KintaiDbContext context, ApplicationDbContext application)
        {
            _context = context;
            context1 = application;
        }
        public void OnGet()
        { 
            Data.Clear();         
        }

        public void OnPost()
        {
            // フォームからValueを受け取る。
            Shain_no = Request.Form["shain_no"];
            Name_mei = Request.Form["name_mei"];
            Shozoku_nm = Request.Form["shozoku_nm"];
            Shokushu_nm = Request.Form["shokushu_nm"];
            Koyokeitai_nm = Request.Form["koyokeitai_nm"];
            // 所属コード、所属コード、雇用形態コードでJOIN
            var no = from a in _context.shain
                     join b in _context.shozoku
                     on a.shozoku_cd equals b.shozoku_cd
                     join c in _context.shokushu
                     on a.shokushu_cd equals c.shokushu_cd
                     join d in _context.koyokeitai
                     on a.koyokeitai_cd equals d.koyokeitai_cd
                     orderby a.shain_no    
                     select new {a.shain_no , a.name_mei , b.shozoku_nm , c.shokushu_nm , d.koyokeitai_nm};

            // 条件による検索すること(value＝nullは検索条件にならないこと。)
            if (!string.IsNullOrEmpty(Shain_no))
            {
                no = no.Where(e => e.shain_no.Equals(Shain_no) );
            }

           if (!string.IsNullOrEmpty(Name_mei))
            {
                no = no.Where(e => e.name_mei.Equals(Name_mei));
            }

            if (!string.IsNullOrEmpty(Shozoku_nm))
            {
                no = no.Where(e => e.shozoku_nm.Equals(Shozoku_nm) );
            }

            if (!string.IsNullOrEmpty(Shokushu_nm))
            {
                no = no.Where(e => e.shokushu_nm.Equals(Shokushu_nm) );
            }

            if (!string.IsNullOrEmpty(Koyokeitai_nm))
            {
                no = no.Where(e => e.koyokeitai_nm.Equals(Koyokeitai_nm));
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
