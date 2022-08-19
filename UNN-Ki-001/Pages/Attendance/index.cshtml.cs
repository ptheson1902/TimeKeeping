﻿using Microsoft.AspNetCore.Authorization;
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

        private T_Kinmu? kinmu { get; set; }
        public string? Taikin { get; set; }
        public string? Shukin { get; set; }
        public string? Message { get; set; }

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
       {
        }

        public void OnGet()
        {
            var now = DateTime.Now;
            var shain = GetCurrentUserShainAsync().Result;
            var today = _kintaiDbContext.t_kinmus.Where(a => a.KigyoCd.Equals(shain.KigyoCd) && a.ShainNo.Equals(shain.ShainNo) && a.KinmuDt.Equals(now.ToString("yyyyMMdd"))).FirstOrDefault();
            if (today == null)
            {
                var old = _kintaiDbContext.t_kinmus.Where(a => a.KigyoCd.Equals(shain.KigyoCd) && a.ShainNo.Equals(shain.ShainNo) && int.Parse(a.KinmuDt) < int.Parse(now.ToString("yyyyMMdd"))).OrderByDescending(e => e.KinmuDt).FirstOrDefault();
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

        public void OnPost()
        {
            var shain = GetCurrentUserShainAsync().Result;
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

        private void Start(M_Shain shain)
        {
            var now = DateTime.Now;
            var today = int.Parse(now.ToString("yyyyMMdd"));
            var kinmu = _kintaiDbContext.t_kinmus
            .Where(e => e.KigyoCd.Equals(shain.KigyoCd) && e.KinmuDt.Equals(today) && e.ShainNo.Equals(shain.ShainNo) && e.KinmuFrDate == null)
            .FirstOrDefault();

            // 該当レコードがなかったら新規作成
            if (kinmu == null)
            {
                kinmu = new T_Kinmu(shain.KigyoCd, shain.ShainNo, today.ToString());
            }
        }

        private void End(M_Shain shain)
        {
            var now = DateTime.Now;
            var today = int.Parse(now.ToString("yyyyMMdd"));
            var kinmu = _kintaiDbContext.t_kinmus
                .Where(e => e.KigyoCd.Equals(shain.KigyoCd) && e.ShainNo.Equals(shain.ShainNo) && e.KinmuToDate == null && int.Parse(e.KinmuDt) < today)
                .OrderByDescending(e => e.KinmuDt)
                .FirstOrDefault();

            if (kinmu == null)
            {
                Message = "退勤可能なレコードが存在しません。";
                return;
            }

            kinmu.DakokuFrDate = now;
        }

    }
}
