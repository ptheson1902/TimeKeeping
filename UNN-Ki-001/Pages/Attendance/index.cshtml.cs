using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly KintaiDbContext kintaiDbContext;
        public string? Date { get; private set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext _context, KintaiDbContext context, KintaiDbContext kintaiDbContext)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _logger = logger;
            applicationDbContext = _context;
            this.kintaiDbContext = kintaiDbContext;
        }

        public async void OnGet()
        {
            // テストしまーす
            M_Kinmu test = await kintaiDbContext.m_kinmus.FirstOrDefaultAsync();
            test.UpdateDt = DateTime.UtcNow;
            kintaiDbContext.SaveChanges();

        }
    }
}
