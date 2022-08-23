using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : BasePageModel
    {
        public List<M_Shain> tgtList = new List<M_Shain>();

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // �Ǘ��Ҍ������L��΁A�Z�b�V�������猟���Ώۂ�ǂݎ�胊�X�g�ɒǉ�����
            if (User.IsInRole("Admin"))
            {
                // �����Ώۂ����݂��Ȃ����NotFound
                List<M_Shain>? sesList = HttpContext.Session.GetObject<List<M_Shain>>(Constants.RECORD_SEARCH_LIST);
                if(sesList == null || sesList.Count == 0)
                {
                    return NotFound();
                }

                tgtList.AddRange(sesList);
            }


            return Page();
        }

        public void OnPost()
        {

        }
        /*
        private DateTime GetShimeStartDate()
        {

        }

        private DateTime GetShimeEndDate()
        {

        }
        */
    }
}
