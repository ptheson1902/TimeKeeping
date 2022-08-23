using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;
namespace UNN_Ki_001.Pages.Attendance.Record
{

    [Authorize(Policy = "Rookie")]
    public class SearchEx : BasePageModel
    {
        public SearchEx(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }


        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<M_Shain> _targetList;

        public void OnGet()
        {
            // ��ʌ����̏ꍇ�A���g�݂̂�ǉ����ċΖ��\��
            if (User.IsInRole("Rookie"))
            {
                
            }
        }
        public IActionResult OnPost(string command, int index)
        {
            if (command != null && command.Equals("sub"))
            {
                // �����܂�

                // �Z�b�V�����Ɋi�[���ċΖ��\�y�[�W�֔��
                HttpContext.Session.SetObj(Constants.RECORD_SEARCH_LIST, _targetList);
                HttpContext.Session.SetInt32(Constants.RECORD_SEARCH_CURRENT_INDEX, index);
                return GotoRecord();
            }

            // �Z���N�g
            _targetList = _kintaiDbContext.m_shains
                .Include(shain => shain.Shokushu)
                .Include(shain => shain.Shozoku)
                .Include(shain => shain.Koyokeitai)
                .WhereIf(Input.No != null, shain => shain.ShainNo.Contains(Input.No!))
                .WhereIf(Input.Name != null, shain => (shain.NameSei + shain.NameMei).Contains(Input.Name!))
                .WhereIf(Input.KigyoCd != null, shain => shain.KigyoCd.Contains(Input.KigyoCd!))
                .WhereIf(Input.KoyokeitaiName != null, shain =>     // �ٗp�`��
                    shain.Koyokeitai != null
                    && shain.Koyokeitai.KoyokeitaiNm != null
                    && shain.Koyokeitai.KoyokeitaiNm.Contains(Input.KoyokeitaiName!))
                .WhereIf(Input.ShozokuName != null, shain =>        // ����
                    shain.Shozoku != null
                    && shain.Shozoku.ShozokuNm != null
                    && shain.Shozoku.ShozokuNm.Contains(Input.ShozokuName!))
                .WhereIf(Input.ShokushuName != null, shain =>       // �E��
                    shain.Shokushu != null
                    && shain.Shokushu.ShokushuNm != null
                    && shain.Shokushu.ShokushuNm.Contains(Input.ShokushuName!))
                .ToList();
            return Page();
        }

        public IActionResult GotoRecord()
        {
            return RedirectToPage("/Attendance/Record/Index");
        }


        public class InputModel
        {
            [Display(Name = "��ƃR�[�h")]
            public string? KigyoCd { get; set; }
            [Display(Name = "�Ј��ԍ�")]
            public string? No { get; set; }
            [Display(Name = "����")]
            public string? Name { get; set; }
            [Display(Name = "����")]
            public string? ShozokuName { get; set; }
            [Display(Name = "�E��")]
            public string? ShokushuName { get; set; }
            [Display(Name = "�ٗp�`��")]
            public string? KoyokeitaiName { get; set; }
        }
        
    }
}
