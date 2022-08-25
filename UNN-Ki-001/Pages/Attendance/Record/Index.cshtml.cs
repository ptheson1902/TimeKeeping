using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : BasePageModel
    {
        public List<Kinmuhyo> DataList = new();
        public List<string[]> MKinmuInfoList = new();
        public ShainSearchRecord Target = new();
        public ShainSearchRecordList? TargetList = null;
        public string TargetListJson = "";

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {

        }

        /// <summary>
        /// �Z�b�V��������^�[�Q�b�g��I�o���ċΖ��\��\�����܂��B
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            // �\���Ώۂ��Z�b�V��������m�肵�܂��B
            TargetListJson = HttpContext.Session.GetString(Constants.SEARCH_RECORD_LIST) ?? "";
            TargetList = JsonConvert.DeserializeObject<ShainSearchRecordList>(TargetListJson);
            // �������݂��Ȃ���ΎЈ������֔�΂��č쐬���Ă��炢�܂��B
            if(TargetList == null)
                return RedirectToPage("/Attendance/Record/Search");

            // �\�����e�𐶐����܂��B
            Target = TargetList.Get();
            var mKinmuList = _kintaiDbContext.m_kinmus
                .Where(e => e.KigyoCd == Target.KigyoCd)
                .ToList();
            foreach(var item in mKinmuList)
            {
                MKinmuInfoList.Add(new string[] { item.KigyoCd, item.KinmuCd, item.KinmuNm ?? "���̖��ݒ�" });
            }
            DataList = CreateData(Target, TargetList.CurrentDate);

            return Page();
        }

        /// <summary>
        /// �p�����[�^�[���󂯎���ĉ�ʑJ�ڂ��܂��B
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IActionResult OnPost(string targetListJson, string command = "")
        {
            // �p�����[�^�[���󂯎��܂��B
            TargetListJson = targetListJson;

            // �^�[�Q�b�g���X�g���f�V���A���C�Y���܂��B
            // TODO: �f�V���A���C�Y���s���̃e�X�g�����Ă��������B
            TargetList = JsonConvert.DeserializeObject<ShainSearchRecordList>(TargetListJson);
            // �������݂��Ȃ���ΎЈ������֔�΂��܂��B
            if (TargetList == null)
                return RedirectToPage("/Attendance/Record/Search");

            // �R�}���h��K�p���ĕ\����ύX���܂�
            switch (command)
            {
                case "NextMonth":
                    TargetList.NextMonth();
                    break;
                case "PrevMonth":
                    TargetList.PrevMonth();
                    break;
                case "NextShain":
                    Target = TargetList.Next().Get();
                    break;
                case "PrevShain":
                    Target = TargetList.Prev().Get();
                    break;
                default:
                    Target = TargetList.Get();
                    break;
            }

            // �\�����e�𐶐����܂��B
            DataList = CreateData(Target, TargetList.CurrentDate);
            var mKinmuList = _kintaiDbContext.m_kinmus
            .Where(e => e.KigyoCd == Target.KigyoCd)
            .ToList();
            foreach (var item in mKinmuList)
            {
                MKinmuInfoList.Add(new string[] { item.KigyoCd, item.KinmuCd, item.KinmuNm ?? "���̖��ݒ�" });
            }
            Target = TargetList.Get();

            // �ĂуV���A���C�Y���ăr���[�ɓn���܂�
            // TODO: �V���A���C�Y���s���̃e�X�g�����Ă��������B
            TargetListJson = JsonConvert.SerializeObject(TargetList);

            return Page();
        }

        private List<Kinmuhyo> CreateData(ShainSearchRecord target, DateTime month)
        {
            // ���ʊi�[�p�̕ϐ�
            List<Kinmuhyo> result = new();

            // ��ƃR�[�h�Őݒ��SELECT
            var setting = _kintaiDbContext.mSettings
                .Where(e => e.KigyoCd.Equals(target.KigyoCd))
                .FirstOrDefault();
            // �ݒ肩����ߓ��𒊏o�i�����l: 99(����))
            int shimebi = (setting == null || setting.ShimeDt == null) ? 99 : (int)setting.ShimeDt;

            // ���ߓ�����������ߓ��܂ł�KinmuDt�ꗗ���擾
            DayList calender = new(month, shimebi);
            var kinmuDtList = calender.KinmuDtList;

            // �Ζ��f�[�^����荞��
            var tempList = _kintaiDbContext.t_kinmus
                .Include(e => e.MKinmu)
                .Where(e => e.KigyoCd!.Equals(target.KigyoCd)
                    && e.ShainNo!.Equals(target.ShainNo)
                    && kinmuDtList.Contains(e.KinmuDt!))
                .OrderBy(e => e.KinmuDt)
                .ToList();

            // ���݂��Ȃ��f�[�^�����߂ďo�͗p�̐��`�ς݃f�[�^���쐬����
            CultureInfo Japanese = new CultureInfo("ja-JP");
            foreach (var kinmuDt in kinmuDtList)
            {
                var data = new Kinmuhyo(kinmuDt);
                var day = DateTime.ParseExact(kinmuDt, "yyyyMMdd", null); // �ϐ�kinmuDt��yyyyMMdd�̃t�H�[�}�b�g�ł��邱�Ƃ�DayList�N���X�ɂ���ĕۏ؂���Ă���
                data.Day = day.ToString("MM��dd��(ddd)", Japanese);

                // �Ζ����R�[�h���擾
                var kinmu = tempList
                    .Where(e => e.KinmuDt!.Equals(kinmuDt))
                    .FirstOrDefault();

                // null�Ȃ�continue(�����l�̓R���X�g���N�^�ő}���ς݁j
                if (kinmu == null)
                {
                    result.Add(data);
                    continue;
                }

                // �Ζ��R�[�h
                if (kinmu.KinmuCd != null)
                    data.KinmuCd = kinmu.KinmuCd;

                // �Ζ��\��
                if (kinmu.MKinmu != null && kinmu.MKinmu.KinmuNm != null)
                    data.Yote = kinmu.MKinmu.KinmuNm;

                // �����t�H�[�}�b�g��錾
                string timeFormat = "HH:mm";

                // ����
                if (kinmu.DakokuFrDate != null)
                    data.DakokuStart = ((DateTime)kinmu.DakokuFrDate).ToString(timeFormat);
                if (kinmu.DakokuToDate != null)
                    data.DakokuEnd = ((DateTime)kinmu.DakokuToDate).ToString(timeFormat);
                if (kinmu.KinmuFrDate != null)
                    data.KinmuStart = ((DateTime)kinmu.KinmuFrDate).ToString(timeFormat);
                if (kinmu.KinmuToDate != null)
                    data.KinmuEnd = ((DateTime)kinmu.KinmuToDate).ToString(timeFormat);

                // �x�e
                if (kinmu.Kyukei != null)
                    data.Kyukei = MinutesToString((int)kinmu.Kyukei);

                //���J��
                if (kinmu.Sorodo != null)
                    data.Sorodo = MinutesToString((int)kinmu.Sorodo);

                // �T��
                if (kinmu.Kojo != null)
                    data.Kojo = MinutesToString((int)kinmu.Kojo);

                // ���l
                if (kinmu.Biko != null)
                    data.Biko = kinmu.Biko;

                // �쐬�����E���X�g�ɒǉ�
                result.Add(data);
            }

            return result;
        }

        public string MinutesToString(int val)
        {
            int hour = val / 60;
            int min = val % 60;
            return hour.ToString("00") + ":" + min.ToString("00");
        }
    }

    public class Kinmuhyo
    {
        public Kinmuhyo(string kinmuDt)
        {
            Day = KinmuCd
                = DakokuStart
                = DakokuEnd
                = KinmuStart
                = KinmuEnd
                = Kyukei
                = Sorodo
                = Kojo
                = Biko
                = "";
            Yote = "���ݒ�";
            KinmuDt = kinmuDt;
        }
        public string KinmuCd { get; set; }
        public string Day { get; set; }
        public string KinmuDt;
        public string Yote { get; set; }
        public string DakokuStart { get; set; }
        public string DakokuEnd { get; set; }
        public string KinmuStart { get; set; }
        public string KinmuEnd { get; set; }
        public string Kyukei { get; set; }
        public string Kojo { get; set; }
        public string Sorodo { get; set; }
        public string Biko { get; set; }
    }


    /// <summary>
    /// ���t���w�肷��Ƃ��̌��̃J�����_�[�𐶐�����I�u�W�F�N�g�ł��B
    /// </summary>
    class DayList
    {
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;
        public readonly int Count;
        public readonly List<string> KinmuDtList; 

        public DayList(DateTime date, int shimebi)
        {
            // ���ߓ���99���w�肳�ꂽ�ꍇ�́A�������Ӗ�����̂Ŋ��荞�ݏ���
            if (shimebi == 99)
            {
                shimebi = 0;
            }
            // C#��DateTime��0�ȉ��̓��t�ɑΉ��ł��Ȃ����߁A���̏ꍇ�͌ォ�獷�������Z���邱�ƂőΉ����܂��B
            // ����āA�O�ȉ��̏ꍇ��1�Ƃ̍����v�����܂�
            int mainasu = 0;
            if (shimebi < 1)
            {
                mainasu = shimebi - 1;
                shimebi = 1;
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
            StartDate = StartDate.AddDays(1 + mainasu);
            EndDate = new DateTime(endYear, endMonth, shimebi);
            EndDate = EndDate.AddDays(mainasu);

            // �������擾���Ċi�[
            Count = (EndDate - StartDate).Days + 1;

            // �J�����_�[�`���ŋΖ��L�^���擾
            KinmuDtList = GetKinmuDtList(StartDate, Count);
        }

        public static List<string> GetKinmuDtList(DateTime startDay, int length)
        {
            // kinmuDt�̃��X�g���쐬
            List<String> kinmuDtList = new List<string>();
            for(int i=0; i<length; i++)
            {
                kinmuDtList.Add(startDay.AddDays(i).ToString("yyyyMMdd"));
            }

            return kinmuDtList;
        }
    }
}
