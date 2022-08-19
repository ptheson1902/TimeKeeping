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
    public class GetDataModel : PageModel
    {
        public List<T_Kinmu?> ListKinmu { get; set; }
        private readonly KintaiDbContext _kintaiDbContext;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public GetDataModel(KintaiDbContext kintaiDbContext)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _kintaiDbContext = kintaiDbContext;
        }
        public JsonResult OnGet(DateTime? month)
        {
            DateTime now = DateTime.Now;
            if (month != null)
            {
                return new JsonResult(_kintaiDbContext.t_kinmus.Where(a => a.ShainNo.Equals(User.Identity.Name) && a.KinmuDt.Substring(0, 6).Equals(((DateTime)month).ToString("yyyyMM"))).Select(a => new {a.KinmuDt, a.DakokuFrDate, a.KinmuFrDate, a.DakokuToDate, a.KinmuToDate}).OrderBy(a => a.KinmuDt));
            }
            else
            {
                return new JsonResult(_kintaiDbContext.t_kinmus.Where(a => a.ShainNo.Equals(User.Identity.Name) && a.KinmuDt.Substring(0, 6).Equals(now.ToString("yyyyMM"))).Select(a => new { a.KinmuDt, a.DakokuFrDate, a.KinmuFrDate, a.DakokuToDate, a.KinmuToDate }).OrderBy(a => a.KinmuDt));
            }
        }
    }
}
