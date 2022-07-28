using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UNN_Ki_001.Areas.Kintai.Pages
{
    [Authorize(Roles = "Admin")]
    public class BaseSettingModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
