using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

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
            
            // é–àıî‘çÜÇ≈åüçı
            /*var no = from a in _context.user
                     where a.shain_no == shain_no
                     select a;*/
            GetData = _context.shain.ToList();
        }
    }
}
