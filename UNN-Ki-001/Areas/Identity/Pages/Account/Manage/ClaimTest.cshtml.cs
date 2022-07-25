using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace UNN_Ki_001.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Manager")]
    public class ClaimTestModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost()
        {
            
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public string RoleName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string ClaimName { get; set; }
        }
    }
}
