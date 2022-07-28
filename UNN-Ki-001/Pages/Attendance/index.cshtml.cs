using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace UNN_Ki_001.Pages.Attebdabce
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
