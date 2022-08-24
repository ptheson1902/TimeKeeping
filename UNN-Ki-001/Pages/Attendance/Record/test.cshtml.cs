using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    public class testModel : PageModel
    {
        private readonly UNN_Ki_001.Data.KintaiDbContext _context;

        public testModel(UNN_Ki_001.Data.KintaiDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public M_Kinmu M_Kinmu { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.m_kinmus == null || M_Kinmu == null)
            {
                return Page();
            }

            _context.m_kinmus.Add(M_Kinmu);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
