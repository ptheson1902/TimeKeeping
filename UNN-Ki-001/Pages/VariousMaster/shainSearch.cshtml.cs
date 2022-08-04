using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UNN_Ki_001.Pages.VariousMaster
{
    [AllowAnonymous]
    public class shainSearchModel : PageModel
    {
        public string? Test { get; set; }
        public void OnGet()
        {
            Test = "";
        }
        public void OnPost(string shain_no)
        {
            Test = shain_no;
        }
    }
}
