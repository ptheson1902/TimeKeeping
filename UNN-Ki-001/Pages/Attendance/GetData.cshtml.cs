using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UNN_Ki_001.Data.Models;
using UNN_Ki_001.Data;
using Microsoft.AspNetCore.Authorization;

namespace UNN_Ki_001.Pages.Attendance
{
    [Authorize(Policy = "Rookie")]
    public class GetDataModel : PageModel
    {
        public List<T_Kinmu?> ListKinmu { get; set; }
        private readonly KintaiDbContext _kintaiDbContext;
        public string? Message { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public GetDataModel(KintaiDbContext kintaiDbContext)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _kintaiDbContext = kintaiDbContext;
        }

        public IActionResult OnGet()
        {
            DateTime now = DateTime.Now;
            //ListKinmu = _kintaiDbContext.t_kinmus.Where(e => e.ShainNo.Equals(User.Identity.Name) && e.KinmuDt.Substring(0, 6).Equals(now.ToString("yyyyMM")));
            return new OkObjectResult(_kintaiDbContext.t_kinmus.Where(e => e.ShainNo.Equals(User.Identity.Name) && e.KinmuDt.Substring(0, 6).Equals(now.ToString("yyyyMM"))));
        }
    }
}
