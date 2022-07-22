using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UNN_Ki_001.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = "EmployeeOnlyaaa")]
    public class Index1Model : PageModel
    {
        [Authorize(Policy = "EmployeeOnlyaaa")]
        public void OnGet()
        {
        }
    }
}
