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
        public IList<Users> GetData { get; set; }
        public Users InsData { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public shainSearchModel(UNN_Ki_001.Data.ApplicationDbContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _context = context;
        }
        public string? SearchString { get; set; }
        public async Task OnGetAsync(string shain_no)
        {
            Test = shain_no;
            var qr = from a in _context.user
                     where a.id == shain_no
                     select a;
            GetData = await qr.ToListAsync();   
        }
         public string? Test { get; set; }
         /*public void OnPost(string shain_no)
         {
            Test = shain_no;
         }*/
    }
}
