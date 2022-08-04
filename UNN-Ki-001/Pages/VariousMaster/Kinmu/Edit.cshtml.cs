using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster.Kinmu
{
    [Authorize(Policy = "Rookie")]
    public class EditModel : PageModel
    {
        private readonly UNN_Ki_001.Data.ApplicationDbContext _context;

        public EditModel(UNN_Ki_001.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public M_Kinmu M_Kinmu { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string KinmuCd, string KigyoCd)
        {
            if (KigyoCd == null || KinmuCd == null || _context.m_Kinmus == null)
            {
                return NotFound();
            }

            var m_kinmu =  await _context.m_Kinmus.FirstOrDefaultAsync(m => m.KigyoCd == KigyoCd && m.KinmuCd == KinmuCd);
            if (m_kinmu == null)
            {
                return NotFound();
            }
            M_Kinmu = m_kinmu;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(M_Kinmu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_KinmuExists(M_Kinmu.KigyoCd))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool M_KinmuExists(string id)
        {
          return (_context.m_Kinmus?.Any(e => e.KigyoCd == id)).GetValueOrDefault();
        }
    }
}
