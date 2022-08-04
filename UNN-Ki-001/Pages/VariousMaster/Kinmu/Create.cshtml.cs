using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster.Kinmu
{
    [Authorize(Policy = "Rookie")]
    public class CreateModel : PageModel
    {
        private readonly UNN_Ki_001.Data.ApplicationDbContext _context;

        public CreateModel(UNN_Ki_001.Data.ApplicationDbContext context)
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
          if (!ModelState.IsValid || _context.m_Kinmus == null || M_Kinmu == null)
            {
                return Page();
            }

            _context.m_Kinmus.Add(M_Kinmu);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
