using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace UNN_Ki_001.Areas.Kintai
{
    [Authorize(Policy = "Rookie")]
    public class DakokuModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
