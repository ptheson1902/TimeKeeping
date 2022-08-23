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
    public class Search : BasePageModel
    {
        public Search(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }


        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<M_Shain> _targetList = new List<M_Shain>();

        private static readonly string _TEMP_SEARCH_RESULT_LIST = "tempsearchresultlistsearch";

        public IActionResult OnGet()
        {
            // ��ʌ����̏ꍇ�A���g�݂̂�ǉ����ċΖ��\��
            if (User.IsInRole("Rookie"))
            {
                // ���g�݂̂��ǉ����ꂽ���X�g���쐬
                var me = GetCurrentUserShainAsync().Result;
                _targetList.Add(me);
                var tempList = CreateRecordList(_targetList);
                // �Z�b�V�����Ɋi�[���ċΖ��\�y�[�W�֔��
                HttpContext.Session.SetObj(Constants.RECORD_SEARCH_LIST, tempList);
                HttpContext.Session.SetInt32(Constants.RECORD_SEARCH_CURRENT_INDEX, 0);
                return GotoRecord();
            }
            return Page();
        }
        public IActionResult OnPost(string command, int index)
        {
            if (command != null && command.Equals("sub"))
            {
                // �ꎞ�f�[�^���Z�b�V��������擾
                var getData = HttpContext.Session.GetObject<List<ShainSearchRecord>>(_TEMP_SEARCH_RESULT_LIST);
                // �ꎞ�f�[�^���Z�b�V��������폜
                HttpContext.Session.Remove(_TEMP_SEARCH_RESULT_LIST);
                // ����f�[�^���Z�b�V�����Ɋi�[���ċΖ��\�y�[�W�֔��
                return GotoRecord(getData, index
                    );
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

            var tempList = CreateRecordList(_targetList);
            HttpContext.Session.SetObj(_TEMP_SEARCH_RESULT_LIST, tempList);


            return Page();
        }

        private List<ShainSearchRecord> CreateRecordList(List<M_Shain> shainList)
        {
            List<ShainSearchRecord> tempList = new();
            foreach (var item in shainList)
            {
                ShainSearchRecord temp = new();
                temp.KigyoCd = item.KigyoCd;
                temp.ShainNo = item.ShainNo;
                temp.ShainNm = (item.NameSei + item.NameMei).Replace(" ", "");
                temp.ShokushuCd = item.ShokushuCd;
                if (item.Shokushu != null)
                {
                    temp.ShokushuNm = item.Shokushu.ShokushuNm;
                }
                temp.ShozokuCd = item.ShozokuCd;
                if (item.Shozoku != null)
                {
                    temp.ShozokuNm = item.Shozoku.ShozokuNm;
                }
                temp.KoyokeitaiCd = item.KoyokeitaiCd;
                if (item.Koyokeitai != null)
                {
                    temp.KoyokeitaiNm = item.Koyokeitai.KoyokeitaiNm;
                }
                tempList.Add(temp);
            }
            return tempList;
        }

        public IActionResult GotoRecord(List<ShainSearchRecord> list, int currentIndex)
        {
            // ����f�[�^���Z�b�V�����Ɋi�[���ċΖ��\�y�[�W�֔��
            HttpContext.Session.SetObj(Constants.RECORD_SEARCH_LIST, list);
            HttpContext.Session.SetInt32(Constants.RECORD_SEARCH_CURRENT_INDEX, currentIndex);
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
