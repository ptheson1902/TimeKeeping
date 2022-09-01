using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    public class TkinmuModel : PageModel
    {
        public JsonResult OnGet(string kinmuDt = "", string shainNo = "", string kigyoCd = "")
        {
            // ...
            return new JsonResult("{}");
        }
    }
}
