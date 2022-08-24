﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    public class listModel : PageModel
    {
        private readonly UNN_Ki_001.Data.KintaiDbContext _context;

        public listModel(UNN_Ki_001.Data.KintaiDbContext context)
        {
            _context = context;
        }

        public IList<M_Kinmu> M_Kinmu { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.m_kinmus != null)
            {
                M_Kinmu = await _context.m_kinmus.ToListAsync();
            }
        }
    }
}
