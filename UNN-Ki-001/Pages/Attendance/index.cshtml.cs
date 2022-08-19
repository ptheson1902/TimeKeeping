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
        private M_Shain? Shain { get; }
        public string? Message { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext applicationDbContext, KintaiDbContext context, KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager)
       {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _kintaiDbContext = kintaiDbContext;
            _userManager = userManager;
        }





        private async Task<M_Shain?> GetCurrentUserShainAsync()
        {
            if(User == null || User.Identity == null)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var shain = _kintaiDbContext.m_shains
                .Where(e => e.KigyoCd.Equals(user.Kigyo_cd) && e.ShainNo.Equals(user.Shain_no))
                .FirstOrDefault();
            return shain;
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
                Message = "打刻中に失敗しました。";
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

        private  void Start(M_Shain shain)
        {
            var now = DateTime.Now;
            var today = int.Parse(now.ToString("yyyyMMdd"));
            var kinmu = _kintaiDbContext.t_kinmus
            .Where(e => e.KigyoCd.Equals(shain.KigyoCd) && e.KinmuDt.Equals(today) && e.ShainNo.Equals(shain.ShainNo) && e.KinmuFrDate == null)
            .FirstOrDefault();

            // 該当レコードがなかったら新規作成
            if(kinmu == null)
            {
                kinmu = new T_Kinmu(shain.KigyoCd, shain.ShainNo, today.ToString());
            }
        }
    }
}
