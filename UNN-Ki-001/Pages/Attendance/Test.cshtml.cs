using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance
{
    [Authorize(Policy = "Rookie")]
    public class TestModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public List<Users> GetData { get; set; }
        [BindProperty]
        public Users InsData { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TestModel(ApplicationDbContext _context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            db = _context;
        }
        public async Task OnGetAsync()
        {
            var qr = from a in db.user
                     where a.user_id == "hoang"
                     select a;
            GetData = await qr.ToListAsync();
        }
        public async Task OnPostAsync()
        {
            var action = Request.Form["action"];
            var user_id = Request.Form["user_id"];
            switch (action)
            {
                case "insert":
                    db.Add(InsData);
                    await db.SaveChangesAsync();
                    break;
                case "delete":
#pragma warning disable CS8601 // Possible null reference assignment.
                    InsData = await db.user.FindAsync(user_id);
#pragma warning restore CS8601 // Possible null reference assignment.
                    db.Remove(InsData);
                    await db.SaveChangesAsync();
                    GetData = db.user.ToList();
                    break;
                default: break;
            };
        }
    }
}
