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
            // �t�H�[������Value���󂯎��B
            string shain_no = Request.Form["shain_no"];
            string name_mei = Request.Form["name_mei"];
            string shozoku_nm = Request.Form["shozoku_cd"];
            string shokushu_nm = Request.Form["shokushu_cd"];
            string koyokeitai_nm = Request.Form["koyokeitai_cd"];
            // �����R�[�h�A�����R�[�h�A�ٗp�`�ԃR�[�h��JOIN
            var no = from a in _context.m_shains
                     join b in _context.m_shozokus
                     on a.shokushu_cd equals b.ShozokuCd
                     join c in _context.m_shokushus
                     on a.shokushu_cd equals c.ShokushuCd
                     join d in _context.m_koyokeitais
                     on a.koyokeitai_cd equals d.KoyokeitaiCd
                     select new {a.shain_no , a.name_mei , b.ShozokuNm , c.ShokushuNm , d.KoyokeitaiNm};

            // �����ɂ�錟�����邱��(value��null�͌��������ɂȂ�Ȃ����ƁB)
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
                no = no.Where(e => e.ShozokuNm.Equals(shozoku_nm) );
            }

            if (!string.IsNullOrEmpty(shokushu_nm))
            {
                no = no.Where(e => e.ShokushuNm.Equals(shokushu_nm) );
            }

            if (!string.IsNullOrEmpty(koyokeitai_nm))
            {
                no = no.Where(e => e.KoyokeitaiNm.Equals(koyokeitai_nm));
            }


            foreach (var item in no)
            {
                Display d = new Display();
                d.shain_no = item.shain_no;
                d.name_mei = item.name_mei;
                d.shozoku_nm = item.ShozokuNm;
                d.shokushu_nm = item.ShokushuNm;
                d.koyokeitai_nm = item.KoyokeitaiNm;

                Data.Add(d);
            }
        }
    }
}
