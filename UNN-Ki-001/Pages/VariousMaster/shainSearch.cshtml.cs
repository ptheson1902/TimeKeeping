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
        public List<m_kensakushain> GetData { get; set; }
        public shainSearchModel(UNN_Ki_001.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public string? SearchString { get; set; }
        public void OnGet()
        { 
        }

        public async Task OnPostAsync()
        {
            string shain_no = Request.Form["shain_no"];
            string name_mei = Request.Form["name_mei"];
            string shozoku_cd = Request.Form["shozoku_cd"];
            string shokushu_cd = Request.Form["shokushu_cd"];
            string koyokeitai_cd = Request.Form["koyokeitai_cd"];            
            // ðŒ–³‚µ‚ÅŒŸõ‚·‚é‚±‚ÆB
            var no = from a in _context.shain
                     select a;

            if (!string.IsNullOrEmpty(shain_no))
            {
                no = no.Where(e => e.shain_no == shain_no);
            }

            if (!string.IsNullOrEmpty(name_mei))
            {
                no = no.Where(e => e.name_mei == name_mei);
            }

            if (!string.IsNullOrEmpty(shozoku_cd))
            {
                no = no.Where(e => e.shozoku_cd == shozoku_cd);
            }

            if (!string.IsNullOrEmpty(shokushu_cd))
            {
                no = no.Where(e => e.shokushu_cd == shokushu_cd);
            }

            if (!string.IsNullOrEmpty(koyokeitai_cd))
            {
                no = no.Where(e => e.koyokeitai_cd == koyokeitai_cd);
            }

            GetData = await no.ToListAsync();

           
        }
    }
}
