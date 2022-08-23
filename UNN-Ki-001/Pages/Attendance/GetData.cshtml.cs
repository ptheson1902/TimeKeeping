using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data.Models;
using UNN_Ki_001.Data;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace UNN_Ki_001.Pages.Attendance
{
    [Authorize(Policy = "Rookie")]
    public class GetDataModel : BasePageModel
    {
        public GetDataModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public JsonResult? OnGet(DateTime? month)
        {
            var shain = GetCurrentUserShainAsync().Result;
            if (shain == null)
                return null;
            if (month != null)
            {
                return new JsonResult(_kintaiDbContext.t_kinmus
                    .Where(a => a.ShainNo!.Equals(shain!.ShainNo) 
                    && a.KigyoCd!.Equals(shain!.KigyoCd) 
                    && a.KinmuDt!.Substring(0, 6).Equals(((DateTime)month).ToString("yyyyMM")))
                    .Select(a => new { a.KinmuDt, a.KinmuFrDate, a.KinmuToDate })
                    .OrderBy(a => a.KinmuDt));
            }
            else
            {
                return new JsonResult(_kintaiDbContext.t_kinmus
                    .Where(a => a.ShainNo!.Equals(shain!.ShainNo) 
                    && a.KigyoCd!.Equals(shain!.KigyoCd) 
                    && a.KinmuDt!.Substring(0, 6).Equals(DateTime.Now.ToString("yyyyMM")))
                    .Select(a => new { a.KinmuDt, a.KinmuFrDate, a.KinmuToDate })
                    .OrderBy(a => a.KinmuDt));
            }
        }
    }
}
