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
        private T_Kinmu? kinmu { get; set; }
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
#pragma warning disable CS8604 // Possible null reference argument.
                kinmu = _kintaiDbContext.t_kinmus
                .Where(e => e.KigyoCd.Equals(user.Kigyo_cd) && e.KinmuDt.Equals(now.ToString("yyyyMMdd")) && e.ShainNo.Equals(user.Shain_no) && e.DakokuFrTm != null && e.DakokuToTm == null)
                .FirstOrDefault();
#pragma warning restore CS8604 // Possible null reference argument.

                kinmu.DakokuEnd();
                kinmu.UpdateDt = DateTime.UtcNow;
                kinmu.UpdateUsr = user.Shain_no;
                _kintaiDbContext.Update(kinmu);
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
                T_Kinmu? t_Kinmu = _kintaiDbContext.t_kinmus
                .Where(e => e.KigyoCd.Equals(user.Kigyo_cd) && e.KinmuDt.Equals(now.ToString("yyyyMMdd")) && e.ShainNo.Equals(user.Shain_no) && e.DakokuFrTm == null)
                .OrderByDescending(e => e.KinmuDt)
                .FirstOrDefault();
                if (t_Kinmu == null)
                    kinmu = new(user.Kigyo_cd, user.Shain_no, now.ToString("yyyyMMdd"), _kintaiDbContext);
                else
                    kinmu = t_Kinmu;
                kinmu.DakokuStart();
                kinmu.CreateUsr = user.Shain_no;
                kinmu.UpdateDt = DateTime.UtcNow;
                kinmu.UpdateUsr = user.Shain_no;
                if (t_Kinmu == null)
                    _kintaiDbContext.Add(kinmu);
                else
                    _kintaiDbContext.Update(kinmu);
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
