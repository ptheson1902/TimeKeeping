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
        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public IActionResult OnGet()
        {
            // �Q�ƃ��X�g���Z�b�V��������ǂݍ���
            var temp = HttpContext.Session.GetObject<ShainSearchRecordList>(Constants.SEARCH_RECORD_LIST);

            // �Q�ƑΏۂ��m�F�ł��Ȃ����NotFound
            if(temp == null)
            {
                // �������`�F�b�N
                var length = temp.List.Count;
                var index = temp.CurrentIndex;
                if(length == 0 || index < 0 || index >= length)
                {
                    return NotFound();
                }
            }

            // �m�F���ς񂾂̂�NULL�񋖗e�^�֍Ĕz�u���܂�
            ShainSearchRecordList targetList = temp;

            // �J�����g�C���f�b�N�X�̏������o���A�r���[�ɓn���܂��B
            ShainSearchRecord target = targetList.GetCurrent();
            ViewData["target"] = target;


            // ��ƃR�[�h�Őݒ��ǂݍ���
            

            // �J�n�����擾

            // �I�������擾

            // �Ζ��f�[�^����荞��
            
            // ���݂��Ȃ��f�[�^�𖄂߂�

            // View�ɓn��

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

    /// <summary>
    /// ���t���w�肷��Ƃ��̌��̃J�����_�[�𐶐�����I�u�W�F�N�g�ł��B
    /// </summary>
    class Kinmuhyo
    {
        public DateTime StartDate;
        public DateTime EndDate;

        public Kinmuhyo(DateTime date, int shimebi)
        {
            // ���ߓ���99���w�肳�ꂽ�ꍇ�́A�������Ӗ�����̂Ŋ��荞�ݏ���
            if (shimebi == 99)
            {
                shimebi = 0;
            }

            // �J�n�����v�Z
            var startYear = date.Year;
            var startMonth = date.Month;


            // ���ߓ����v�Z
            var endDate = date.AddMonths(1);
            var endYear = endDate.Year;
            var endMonth = endDate.Month;

            // ���t�^���쐬
            StartDate = new DateTime(startYear, startMonth, shimebi);
            StartDate = StartDate.AddDays(1);
            EndDate = new DateTime(endYear, endMonth, shimebi);

        }
    }
}
