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
        public List<string> tgtList = new List<string>();

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // �Ǘ��Ҍ������L��΁A�Z�b�V�������猟���Ώۂ�ǂݎ�胊�X�g�ɒǉ�����
            if (User.IsInRole("Admin"))
            {
                // �����Ώۂ����݂��Ȃ����NotFound
                List<string>? sesList = HttpContext.Session.GetObject<List<string>>("target");
                if(sesList == null || sesList.Count == 0)
                {
                    return NotFound();
                }

                tgtList.AddRange(sesList);
            }
            // ��ʌ����ŗL��΁A�������g�����X�g�ɒǉ�
            else
            {
                var me = GetCurrentUserShainAsync().Result;
                
                // �Ј����ƕR�Â��ĂȂ���΁A�g�b�v�y�[�W�փ��_�C���N�g
                if(me == null)
                {
                    return RedirectToPage("/");
                }

                tgtList.Add(me.ShainNo);
            }


            return Page();
        }

        public void OnPost()
        {

        }
    }
}
