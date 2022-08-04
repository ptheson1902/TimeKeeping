﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.VariousMaster.Kinmu
{
    [Authorize(Policy = "Rookie")]
    public class DeleteModel : PageModel
    {
        private readonly UNN_Ki_001.Data.ApplicationDbContext _context;

        public DeleteModel(UNN_Ki_001.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public M_Kinmu M_Kinmu { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string KinmuCd, string KigyoCd)
        {
            if (KinmuCd == null || KigyoCd == null || _context.m_Kinmus == null)
            {
                return NotFound();
            }

            var m_kinmu = await _context.m_Kinmus.FirstOrDefaultAsync(m => m.KigyoCd == KigyoCd && m.KinmuCd == KinmuCd);

            if (m_kinmu == null)
            {
                return NotFound();
            }
            else 
            {
                M_Kinmu = m_kinmu;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.m_Kinmus == null)
            {
                return NotFound();
            }
            var m_kinmu = await _context.m_Kinmus.FindAsync(id);

            if (m_kinmu != null)
            {
                M_Kinmu = m_kinmu;
                _context.m_Kinmus.Remove(M_Kinmu);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
