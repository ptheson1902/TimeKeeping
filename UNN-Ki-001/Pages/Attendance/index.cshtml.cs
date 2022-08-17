using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Diagnostics;
using System.Linq;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Control;
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
        public string? IsTaikin { get; set; }
        public string? IsShukin { get; set; }
        public List<T_Kinmu?> ListKinmu { get; set; }
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

        public async Task OnGetAsync()
        {
            var now = DateTime.Now;
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            var today = _kintaiDbContext.t_kinmus.Where(a => a.KigyoCd.Equals(user.Kigyo_cd) && a.ShainNo.Equals(user.Shain_no) && a.KinmuDt.Equals(now.ToString("yyyyMMdd"))).FirstOrDefault();
            if (today == null)
            {
                var old = _kintaiDbContext.t_kinmus.Where(a => a.KigyoCd.Equals(user.Kigyo_cd) && a.ShainNo.Equals(user.Shain_no)).OrderByDescending(e => e.KinmuDt).FirstOrDefault();
                if((old.DakokuFrDt != null && old.DakokuFrTm != null && old.DakokuToDt != null && old.DakokuToTm != null) ||(old.DakokuFrDt == null && old.DakokuFrTm == null && old.DakokuToDt == null && old.DakokuToTm != null))
                {
                    IsShukin = null;
                    IsTaikin = "disabled";
                }
            }
            else
            {
                if(today.DakokuFrDt != null && today.DakokuFrTm != null && today.DakokuToDt == null && today.DakokuToTm == null){
                    IsShukin = "disabled";
                    IsTaikin = null;
                }else if(today.DakokuFrDt != null && today.DakokuFrTm != null && today.DakokuToDt != null && today.DakokuToTm != null)
                {
                    IsShukin = "disabled";
                    IsTaikin = "disabled";
                }
                else
                {
                    IsShukin = null;
                    IsTaikin = "disabled";
                }
            }
        }

        public async Task OnPostAsync()
        {
            var now = DateTime.Now;
            var action = Request.Form["action"];
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            switch (action)
            {
                case "start":
                    await StartAsync(user);
                    await OnGetAsync();
                    break;
                case "end":
                    await EndAsync(user);
                    await OnGetAsync();
                    break;
                default:
                    await OnGetAsync();
                    break;
            }
        }

        private async Task EndAsync(AppUser user)
        {
            var now = DateTime.Now;
            kinmu = _kintaiDbContext.t_kinmus
                .Where(e => e.KigyoCd.Equals(user.Kigyo_cd) && e.ShainNo.Equals(user.Shain_no) && e.DakokuFrDt != null && e.DakokuFrTm != null && e.DakokuToDt == null && e.DakokuToDt == null)
                .OrderByDescending(e => e.KinmuDt)
                .FirstOrDefault();
            if (kinmu != null)
            {
                KinmuControl kc = new KinmuControl(kinmu, _kintaiDbContext);
                kinmu = kc.DakokuEnd();
                kinmu.CreateUsr = user.Shain_no;
                kinmu.UpdateDt = DateTime.UtcNow;
                kinmu.UpdateUsr = user.Shain_no;
                _kintaiDbContext.Update(kinmu);
                await _kintaiDbContext.SaveChangesAsync();
                Message = "退勤が出来ました";
            }
        }

        private async Task StartAsync(AppUser user)
        {
            var now = DateTime.Now;
            kinmu = new(user.Kigyo_cd, user.Shain_no, now.ToString("yyyyMMdd"));
            kinmu.KinmuCd = "K001";
            KinmuControl kc = new KinmuControl(kinmu, _kintaiDbContext);
            kinmu = kc.DakokuStart();
            kinmu.CreateUsr = user.Shain_no;
            kinmu.UpdateDt = DateTime.UtcNow;
            kinmu.UpdateUsr = user.Shain_no;
            _kintaiDbContext.Add(kinmu);
            await _kintaiDbContext.SaveChangesAsync();
            Message = "出勤が出来ました";
        }
    }
}
