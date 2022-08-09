using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Linq;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly KintaiDbContext _kintaiDbContext;
        private readonly UserManager<AppUser> _userManager;
        public string? Message { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext applicationDbContext, KintaiDbContext context, KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _kintaiDbContext = kintaiDbContext;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            var action = Request.Form["action"];
            switch (action)
            {
                case "start":
                    await StartAsync();
                    break;
                case "end":
                    await EndAsync();
                    break;
                default: break;
            }
        }

        private async Task EndAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            var now = DateTime.Now;
            if (user != null)
            {
                T_Kinmu t_Kinmu = new T_Kinmu(user.Kigyo_cd, user.Shain_no, now.ToString("yyyyMMdd"), _kintaiDbContext);
                t_Kinmu.DakokuEnd();
                t_Kinmu.UpdateDt = DateTime.UtcNow;
                t_Kinmu.UpdateUsr = user.Shain_no;
                    _kintaiDbContext.Update(t_Kinmu);
                _kintaiDbContext.SaveChanges();
                Message = "退勤が出来ました";
            }
            else
            {
                Message = "退勤ができません。";
            }
        }

        private async Task StartAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            var now = DateTime.Now;
            if (user != null)
            {
                T_Kinmu t_Kinmu = new T_Kinmu(user.Kigyo_cd, user.Shain_no, now.ToString("yyyyMMdd"), _kintaiDbContext);
                t_Kinmu.DakokuStart();
                t_Kinmu.CreateUsr = user.Shain_no;
                t_Kinmu.UpdateDt = DateTime.UtcNow;
                t_Kinmu.UpdateUsr = user.Shain_no;
                try
                {
                    _kintaiDbContext.Add(t_Kinmu);
                }
                catch
                {
                    _kintaiDbContext.Update(t_Kinmu);
                }
                _kintaiDbContext.SaveChanges();
                Message = "出勤が出来ました";
            }
            else
            {
                Message = "出勤ができません。";
            }
        }
    }
}
