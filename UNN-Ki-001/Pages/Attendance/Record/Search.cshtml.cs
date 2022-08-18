using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;
namespace UNN_Ki_001.Pages.Attendance.Record
{
    [AllowAnonymous]
    public class Search : PageModel
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

        public Search(UNN_Ki_001.Data.KintaiDbContext context, ApplicationDbContext application)
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
            var no = from a in _context.m_shains
                     join b in _context.m_shozokus
                     on a.ShozokuCd equals b.ShozokuCd
                     join c in _context.m_shokushus
                     on a.ShokushuCd equals c.ShokushuCd
                     join d in _context.m_koyokeitais
                     on a.KoyokeitaiCd equals d.KoyokeitaiCd
                     orderby a.ShainNo    
                     select new {a.ShainNo , a.NameMei , b.ShozokuNm , c.ShokushuNm , d.KoyokeitaiNm};

            // 条件による検索すること(value＝nullは検索条件にならないこと。)
            if (!string.IsNullOrEmpty(Shain_no))
            {
                no = no.Where(e => e.ShainNo.Equals(Shain_no) );
            }

           if (!string.IsNullOrEmpty(Name_mei))
            {
                no = no.Where(e => e.NameMei.Equals(Name_mei));
            }

            if (!string.IsNullOrEmpty(Shozoku_nm))
            {
                no = no.Where(e => e.ShozokuNm.Equals(Shozoku_nm) );
            }

            if (!string.IsNullOrEmpty(Shokushu_nm))
            {
                no = no.Where(e => e.ShokushuNm.Equals(Shokushu_nm) );
            }

            if (!string.IsNullOrEmpty(Koyokeitai_nm))
            {
                no = no.Where(e => e.KoyokeitaiNm.Equals(Koyokeitai_nm));
            }


            foreach (var item in no)
            {
                Display d = new Display();
                d.shain_no = item.ShainNo;
                d.name_mei = item.NameMei;
                d.shozoku_nm = item.ShozokuNm;
                d.shokushu_nm = item.ShokushuNm;
                d.koyokeitai_nm = item.KoyokeitaiNm;
                Data.Add(d);
            }
        }
    }
}
