using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
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
            var today = _kintaiDbContext.t_kinmus.
                Where(e => e.KigyoCd!.Equals(shain.KigyoCd) && e.ShainNo!.Equals(shain.ShainNo) && e.KinmuDt!.Equals(now.ToString("yyyyMMdd")))
                .FirstOrDefault();
            if (today == null || (today.KinmuFrDate == null && today.KinmuToDate == null))
            {
                int d = int.Parse(now.ToString("yyyyMMdd"));
                var old = _kintaiDbContext.t_kinmus
                    .Where(e => e.KigyoCd!.Equals(shain.KigyoCd) && e.ShainNo!.Equals(shain.ShainNo) && Convert.ToInt32(e.KinmuDt) < d)
                    .OrderByDescending(e => e.KinmuDt)
                    .FirstOrDefault();
                if ((old != null && ((old.KinmuFrDate != null && old.KinmuToDate != null) || (old.KinmuFrDate == null && old.KinmuToDate == null))) || old == null)
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
                else if (today.KinmuFrDate != null && today.KinmuToDate != null)
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
            // パターン1のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220801 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220801 23:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン2のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220802 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220802 18:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン3のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220803 09:15:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220803 18:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン4のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220804 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220804 15:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン5のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220805 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220805 15:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン6のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220806 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220806 15:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン7のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220807 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220807 18:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン8のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220808 13:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220808 18:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン9のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220809 13:30:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220809 18:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン10のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220810 13:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220810 17:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン11のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220811 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220811 12:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン12のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220812 09:30:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220812 12:00:00", "yyyyMMdd HH:mm:ss", null);

            // パターン13のテストデーター
            //DateTime dkf = DateTime.ParseExact("20220813 09:00:00", "yyyyMMdd HH:mm:ss", null);
            //DateTime dkt = DateTime.ParseExact("20220813 11:00:00", "yyyyMMdd HH:mm:ss", null);

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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Message = "打刻に失敗しました。";
            }
            finally
            {
                DisabledButton(shain);
            }
        }

        private void End(M_Shain shain)
        {
            // Original
            int kyo = int.Parse(now.ToString("yyyyMMdd"));
            int kino = int.Parse(now.AddDays(-1).ToString("yyyyMMdd"));
            T_Kinmu? kinmu = _kintaiDbContext.t_kinmus
                    .Where(e => e.KigyoCd!.Equals(shain.KigyoCd) && e.ShainNo!.Equals(shain.ShainNo) && e.KinmuFrDate != null && e.KinmuToDate == null && Convert.ToInt32(e.KinmuDt) <= kyo && kino <= Convert.ToInt32(e.KinmuDt))
                    .OrderByDescending(e => e.KinmuDt)
                    .FirstOrDefault();

            if (kinmu == null)
            {
                Message = "退勤可能なレコードが存在しません。";
                return;
            }
            kinmu.DakokuToDate = DateTime.Now;
            _kintaiDbContext.Update(kinmu);
        }

        private void Start(M_Shain shain)
        {
            // Original
            T_Kinmu? kinmu = _kintaiDbContext.t_kinmus
            .Where(e => e.KigyoCd!.Equals(shain.KigyoCd) && e.KinmuDt!.Equals(now.ToString("yyyyMMdd")) && e.ShainNo!.Equals(shain.ShainNo) && e.KinmuFrDate == null)
            .FirstOrDefault();


            // 該当レコードがなかったら新規作成
            if (kinmu == null)
            {
                kinmu = new T_Kinmu(shain.KigyoCd, shain.ShainNo, now.ToString("yyyyMMdd"));
                kinmu.SetKinmuCd("K001"); // TODO: テスト用コード
                _kintaiDbContext.Add(kinmu);
            }
            kinmu.DakokuFrDate = DateTime.Now;
        }
    }
}
