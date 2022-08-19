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
    public class IndexModel : BasePageModel
    {
        public string? Taikin { get; set; }
        public string? Shukin { get; set; }
        public string? Message { get; set; }
        private M_Shain? shain;
        private DateTime now = DateTime.Now;

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }
        public IActionResult OnGet()
        {
            shain = GetCurrentUserShainAsync().Result;
            if (shain == null)
                return RedirectToPage("/");
            else
                DisabledButton(shain);
            return Page();
        }
        private void DisabledButton(M_Shain shain)
        {
            int d = int.Parse(now.ToString("yyyyMMdd"));
            var today = _kintaiDbContext.t_kinmus.
                Where(a => a.KigyoCd.Equals(shain.KigyoCd) && a.ShainNo.Equals(shain.ShainNo) && a.KinmuDt.Equals(now.ToString("yyyyMMdd")))
                .FirstOrDefault();
            if (today == null)
            {
                var old = _kintaiDbContext.t_kinmus
                    .Where(a => a.KigyoCd.Equals(shain.KigyoCd) && a.ShainNo.Equals(shain.ShainNo) && Convert.ToInt32(a.KinmuDt) < d)
                    .OrderByDescending(e => e.KinmuDt)
                    .FirstOrDefault();
                if (old != null && (old.KinmuFrDate != null && old.KinmuToDate != null) || old != null && (old.KinmuFrDate == null && old.KinmuToDate != null))
                {
                    Shukin = null;
                    Taikin = "disabled";
                }
            }
            else
            {
                if (today.KinmuFrDate != null && today.KinmuToDate == null)
                {
                    Shukin = "disabled";
                    Taikin = null;
                }
                else if ( today.KinmuFrDate != null && today.KinmuToDate != null)
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

        public void OnPost()
        {
            shain = GetCurrentUserShainAsync().Result;
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
            DisabledButton(shain);
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
            kinmu.DakokuToDate = DateTime.Now;
        }

        private void Start(M_Shain shain)
        {
            T_Kinmu? kinmu = _kintaiDbContext.t_kinmus
            .Where(e => e.KigyoCd.Equals(shain.KigyoCd) && e.KinmuDt.Equals(now.ToString("yyyyMMdd")) && e.ShainNo.Equals(shain.ShainNo) && e.KinmuFrDate == null)
            .FirstOrDefault();

            // 該当レコードがなかったら新規作成
            if(kinmu == null)
                kinmu = new T_Kinmu(shain.KigyoCd, shain.ShainNo, now.ToString("yyyyMMdd"));
            kinmu.DakokuFrDate = DateTime.Now;
        }
    }
}
