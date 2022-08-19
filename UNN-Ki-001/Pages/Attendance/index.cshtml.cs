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
        //private T_Kinmu? kinmu { get; set; }
        public string? Taikin { get; set; }
        public string? Shukin { get; set; }
        public string? Message { get; set; }
        private DateTime now = DateTime.Now;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext applicationDbContext, KintaiDbContext context, KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager)
       {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _kintaiDbContext = kintaiDbContext;
            _userManager = userManager;
        }

        public void OnGet()
        {
            int d = int.Parse(now.ToString("yyyyMMdd"));
            M_Shain? shain = GetCurrentUserShainAsync().Result;
            var today = _kintaiDbContext.t_kinmus.
                Where(a => a.KigyoCd.Equals(shain.KigyoCd) && a.ShainNo.Equals(shain.ShainNo) && a.KinmuDt.Equals(now.ToString("yyyyMMdd")))
                .FirstOrDefault();
            if (today == null)
            {
                var old = _kintaiDbContext.t_kinmus
                    .Where(a => a.KigyoCd.Equals(shain.KigyoCd) && a.ShainNo.Equals(shain.ShainNo) && Convert.ToInt32(a.KinmuDt) < d)
                    .OrderByDescending(e => e.KinmuDt)
                    .FirstOrDefault();
                if (old != null && (old.DakokuFrDate != null && old.KinmuFrDate != null && old.DakokuToDate != null && old.KinmuToDate != null) || old != null && (old.DakokuFrDate == null && old.KinmuFrDate == null && old.DakokuToDate == null && old.KinmuToDate != null))
                {
                    Shukin = null;
                    Taikin = "disabled";
                }
            }
            else
            {
                if (today.DakokuFrDate != null && today.KinmuFrDate != null && today.DakokuToDate == null && today.KinmuToDate == null)
                {
                    Shukin = "disabled";
                    Taikin = null;
                }
                else if (today.DakokuFrDate != null && today.KinmuFrDate != null && today.DakokuToDate != null && today.KinmuToDate != null)
                {
                    Shukin = "disabled";
                    Taikin = "disabled";
                }
                else
                {
                    Shukin = null;
                    Taikin = "disabled";
                }
            }
        }

        private async Task<M_Shain?> GetCurrentUserShainAsync()
        {
            if (User == null || User.Identity == null)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            M_Shain? shain = _kintaiDbContext.m_shains
                .Where(e => e.KigyoCd.Equals(user.Kigyo_cd) && e.ShainNo.Equals(user.Shain_no))
                .FirstOrDefault();
            return shain;
        }

        public void OnPost()
        {
            M_Shain? shain = GetCurrentUserShainAsync().Result;
            if (shain == null)
            {
                Message = "ユーザーに社員が登録されていません。";
                return;
            }

            var action = Request.Form["action"];
            switch (action)
            {
                case "start":
                    Start(shain);
                    break;
                case "end":
                    End(shain);
                    break;
                default: break;
            }

            try
            {
                _kintaiDbContext.SaveChanges();
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Message = "打刻に失敗しました。";
            }
            OnGet();
        }

        private void End(M_Shain shain)
        {
            int today = int.Parse(now.ToString("yyyyMMdd"));
            T_Kinmu? kinmu = _kintaiDbContext.t_kinmus
                    .Where(a => a.KigyoCd.Equals(shain.KigyoCd) && a.ShainNo.Equals(shain.ShainNo) && a.KinmuFrDate != null && a.KinmuToDate == null && Convert.ToInt32(a.KinmuDt) <= today)
                    .OrderByDescending(e => e.KinmuDt)
                    .FirstOrDefault();

            if (kinmu == null)
            {
                Message = "退勤可能なレコードが存在しません。";
                return;
            }
            kinmu.DakokuFrDate = now;
            _kintaiDbContext.t_kinmus.Update(kinmu);
        }

        private void Start(M_Shain shain)
        {
            T_Kinmu? kinmu = _kintaiDbContext.t_kinmus
            .Where(e => e.KigyoCd.Equals(shain.KigyoCd) && e.KinmuDt.Equals(now.ToString("yyyyMMdd")) && e.ShainNo.Equals(shain.ShainNo) && e.KinmuFrDate == null)
            .FirstOrDefault();

            // 該当レコードがなかったら新規作成
            if(kinmu == null)
            {
                kinmu = new T_Kinmu(shain.KigyoCd, shain.ShainNo, now.ToString("yyyyMMdd"));
                _kintaiDbContext.t_kinmus.Add(kinmu);
            }
            else
            {
                _kintaiDbContext.t_kinmus.Update(kinmu);
            }
        }
    }
}
