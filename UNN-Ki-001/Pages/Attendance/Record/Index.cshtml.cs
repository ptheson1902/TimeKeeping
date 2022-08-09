using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : PageModel
    {
        private readonly UNN_Ki_001.Data.KintaiDbContext _context;
        private readonly KinmuManager kinmuManager;

        public IndexModel(UNN_Ki_001.Data.KintaiDbContext context, KinmuManager kinmuManager)
        {
            _context = context;
            this.kinmuManager = kinmuManager;
        }

        public IList<T_Kinmu> T_Kinmu { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.t_kinmus != null)
            {
                T_Kinmu = await _context.t_kinmus.ToListAsync();
            }
            kinmuManager.Shukkin("test");
        }
    }
}
