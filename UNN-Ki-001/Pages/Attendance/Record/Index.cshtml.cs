// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    [Authorize(Policy = "Rookie")]
    public class IndexModel : BasePageModel
    {
        // �����t�H�[�}�b�g��錾
        public static readonly string TIME_FORMAT = "HH:mm";

        public List<Kinmuhyo> DataList = new();
        public List<string[]> MKinmuInfoList = new();
        public ShainSearchRecord Target = new();
        public ShainSearchRecordList? TargetList = null;
        public string TargetListJson = "";
        public string ShoteiTimes = "";
        public string SorodoTimes = "";
/*      public int? ShoteiDays = 0;
        public int? YasumiDays = 0;
        public int? YukyuDays = 0;*/

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {

        }

        /// <summary>
        /// �Z�b�V��������^�[�Q�b�g��I�o���ċΖ��\��\�����܂��B
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Init();
        }

        /// <summary>
        /// �p�����[�^�[���󂯎���ĉ�ʑJ�ڂ��܂��B
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IActionResult OnPost(string targetListJson = "", string json = "", string kyukeiJson = "", string command = "")
        {
            // �Ǘ��҂���Ȃ���΃p�����[�^�[���㏑������
            if (!User.IsInRole("Admin"))
            {
                targetListJson = "";
                json = "";
                kyukeiJson = "";
            }

            return Init(targetListJson, command, json, kyukeiJson);
        }

        private IActionResult Init(string targetListJson = "", string command = "", string json = "", string kyukeiJson = "")
        {
            if (targetListJson == "")
            {
                // �\���Ώۂ��Z�b�V��������m�肵�܂��B
                TargetListJson = HttpContext.Session.GetString(Constants.SEARCH_RECORD_LIST) ?? "";
            }
            else
            {
                // �\���Ώۂ��p�����[�^�[����擾���܂�
                TargetListJson = targetListJson;
            }
            // �f�V���A���C�Y���s
            TargetList = JsonConvert.DeserializeObject<ShainSearchRecordList>(TargetListJson);

            // �������݂��Ȃ���ΎЈ������֔�΂��č쐬���Ă��炢�܂��B
            if (TargetList == null)
                return RedirectToPage("/Attendance/Record/Search");

            // �R�}���h�������A�^�[�Q�b�g�̏����m�肵�܂��B
            if (command == "NextMonth")
            {
                TargetList.NextMonth();
            }
            else if(command == "NowMonth")
            {
                TargetList.NowMonth();
            }
            else if (command == "PrevMonth")
            {
                TargetList.PrevMonth();
            }
            else if (command == "NextShain")
            {
                Target = TargetList.Next().Get();
            }
            else if(command == "PrevShain")
            {
                Target = TargetList.Prev().Get();
            }
            if(Target.KigyoCd == "" || Target.ShainNo == "")
            {
                Target = TargetList.Get();
            }

            // �x�ejson���w�肳��Ă����json�̏���
            if(kyukeiJson != "")
            {


                // �f�V���A���C�Y����
                var ChangeList = JsonConvert.DeserializeObject<KyukeiRecordJsonList>(kyukeiJson);

                if(ChangeList != null)
                {
                    // ���t���擾
                    var kinmuDt = ChangeList.kinmuDt;
                    // �܂������̋x�e�����ׂč폜����
                    var kyukeis = _kintaiDbContext.t_Kyukeis
                        .Where(e => e.KigyoCd == Target.KigyoCd
                            && e.ShainNo == Target.ShainNo
                            && e.KinmuDt == kinmuDt)
                        .ToList();
                    _kintaiDbContext.RemoveRange(kyukeis);

                    // �Ζ����R�[�h��p�ӂ���
                    var kinmu = _kintaiDbContext.t_kinmus
                        .Where(e => e.KigyoCd == Target.KigyoCd
                            && e.ShainNo == Target.ShainNo
                            && e.KinmuDt == kinmuDt)
                        .FirstOrDefault();
                    if (kinmu == null)
                    {
                        kinmu = new T_Kinmu(Target.KigyoCd, Target.ShainNo, kinmuDt);
                    } else
                    {
                        // �W�v�̏���������
                        if((kinmu.KinmuFrDate != null && kinmu.KinmuToDate != null))
                        {
                            kinmu.ClearInfo();
                        }
                    }

                    // json�����ƂɐV�K�쐬���Ēǉ�
                    int count = 0;
                    foreach (var item in ChangeList.list)
                    {
                        // ������ϊ�
                        DateControl frDc = new DateControl(kinmuDt, item.dakokuFrTm.Replace(":", ""), item.dakokuFrKbn);
                        DateControl toDc = new DateControl(kinmuDt, item.dakokuToTm.Replace(":", ""), item.dakokuToKbn);
                        // �x�e���R�[�h���쐬
                        T_Kyukei kyukei = new(Target.KigyoCd, Target.ShainNo, kinmuDt, count++, (T_Kinmu)kinmu);
                        kyukei.DakokuFrDate = frDc.Origin;
                        kyukei.DakokuToDate = toDc.Origin;

                        // DB�ɒǉ�
                        _kintaiDbContext.Add(kyukei);

                    }
                    _kintaiDbContext.SaveChanges();
                }
            }

            // json���w�肳��Ă����DB��ύX��������
            if (json != "")
            {
                // �f�V���A���C�Y����
                var ChangeList = JsonConvert.DeserializeObject<Dictionary<string, KinmuRecordJson>>(json);
                if (ChangeList != null)
                {
                    foreach (var item in ChangeList)
                    {
                        var kinmuDt = item.Key;
                        var value = item.Value;

                        // ���������Ƀ��R�[�h�����݂���Ύ��񂹂�
                        var kinmu = _kintaiDbContext.t_kinmus
                            .Where(e => e.KigyoCd == Target.KigyoCd
                                && e.ShainNo == Target.ShainNo
                                && e.KinmuDt == kinmuDt)
                            .FirstOrDefault();
                        // �����łȂ���΍��܂�
                        if (kinmu == null)
                        {
                            kinmu = new T_Kinmu();
                            kinmu.KigyoCd = Target.KigyoCd;
                            kinmu.KinmuDt = kinmuDt;
                            kinmu.ShainNo = Target.ShainNo;

                            _kintaiDbContext.Add(kinmu);
                        } else
                        {
                            
                            // �W�v�̏�������K�X�s���܂�
                            if((value.kinmuCd != "" && kinmu.KinmuCd != value.kinmuCd) || (value.kinmuFr == null || value.kinmuTo == null))
                            {
                                kinmu.ClearInfo();
                            }
                        }


                        // �p�����[�^�[�ɏ]���l��ύX
                        //kinmu.DakokuFrDate = value.dakokuFr; // �ō��̂ݕύX�����Reloadable�ɂ���ċΖ��L�^���쐬�����̂�
                        //kinmu.DakokuToDate = value.dakokuTo; // �ꌩ����ƃo�O�Ȃ̂ŁA��������ύX�ł��Ȃ��悤�ɂ��Ă��܂��B
                        kinmu.KinmuFrDate = value.kinmuFr;
                        kinmu.KinmuToDate = value.kinmuTo;
                        kinmu.KinmuCd = value.kinmuCd;
                        kinmu.Biko = value.biko;
                    }   

                    // �ύX��K�p
                    _kintaiDbContext.SaveChanges();
                }
            }

            // View�\������Ζ��\�̓��e�𐶐����܂��B
            DataList = CreateData(Target, TargetList.CurrentDate);
            

            // ����Ɏg���Ζ��}�X�^�̖��̂��擾���܂��B
            var mKinmuList = _kintaiDbContext.m_kinmus
                .Where(e => e.KigyoCd == Target.KigyoCd)
                .ToList();
            foreach (var item in mKinmuList)
            {
                MKinmuInfoList.Add(new string[] { item.KigyoCd, item.KinmuCd, item.KinmuNm ?? "���̖��ݒ�" });
            }

            // �ĂуV���A���C�Y���ăr���[�ɓn���܂�
            // TODO: �V���A���C�Y���s���̃e�X�g�����Ă��������B
            TargetListJson = JsonConvert.SerializeObject(TargetList);
            // ���łɃZ�b�V�������㏑�����܂��B
            HttpContext.Session.SetString(Constants.SEARCH_RECORD_LIST, TargetListJson);

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
            int? shoteiTimes = 0;
            int? sorodoTimes = 0;
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

                // ����
                int standardYM = int.Parse(kinmuDt);

                if (kinmu.DakokuFrDate != null)
                {
                    var datetime = (DateTime)kinmu.DakokuFrDate;
                    data.DakokuStart = datetime.ToString(TIME_FORMAT);
                    // �敪�̌v�Z
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.DakokuFrKbn = (targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }
                if (kinmu.DakokuToDate != null)
                {
                    var datetime = (DateTime)kinmu.DakokuToDate;
                    data.DakokuEnd = datetime.ToString(TIME_FORMAT);
                    // �敪�̌v�Z
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.DakokuToKbn = (targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }
                if (kinmu.KinmuFrDate != null)
                {
                    var datetime = (DateTime)kinmu.KinmuFrDate;

                    data.KinmuStart = datetime.ToString(TIME_FORMAT);
                    // �敪�̌v�Z
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.KinmuFrKbn = ( targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }
                if (kinmu.KinmuToDate != null)
                {
                    var datetime = (DateTime)kinmu.KinmuToDate;
                    data.KinmuEnd = datetime.ToString(TIME_FORMAT);
                    // �敪�̌v�Z
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.KinmuToKbn = (targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }

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
                // ���莞��
                if (kinmu.Shotei != null && kinmu.KinmuFrDate != null && kinmu.KinmuToDate != null)
                    shoteiTimes += kinmu.Shotei;
                // ���J��
                if (kinmu.Sorodo != null && kinmu.KinmuFrDate != null && kinmu.KinmuToDate != null)
                    sorodoTimes += kinmu.Sorodo;
                /*// �������
                if (kinmu.MKinmu != null)
                {
                    if(kinmu.MKinmu.KinmuBunrui ==)
                }
                else
                {
                    YasumiDays++;
                }*/
                // �쐬�����E���X�g�ɒǉ�
                result.Add(data);
            }
            ShoteiTimes = MinutesToString((int)shoteiTimes);
            SorodoTimes = MinutesToString((int)sorodoTimes);

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
            Day = DakokuStart
                = DakokuEnd
                = KinmuStart
                = KinmuEnd
                = Kyukei
                = Sorodo
                = Kojo
                = Yote
                = "N/A";
            Biko = KinmuCd = "";
            KinmuDt = kinmuDt;
        }
        public string KinmuCd { get; set; }
        public string Day { get; set; }
        public string KinmuDt;
        public string Yote { get; set; }
        public string DakokuStart { get; set; }
        public int DakokuFrKbn { get; set; }
        public string DakokuEnd { get; set; }
        public int DakokuToKbn { get; set; }
        public string KinmuStart { get; set; }
        public int KinmuFrKbn { get; set; }
        public string KinmuEnd { get; set; }
        public int KinmuToKbn { get; set; }
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

    [DataContract]
    class KyukeiRecordJsonList
    {
        [DataMember(Name = "kinmuDt")]
        public string kinmuDt { get; set; }

        [DataMember(Name = "list")]
        public List<KyukeiRecordJson> list { get; set; }
    }

    [DataContract]
    class KyukeiRecordJson
    {
        [DataMember(Name ="dakokuFrTm")]
        public string dakokuFrTm { get; set; }
        [DataMember(Name = "dakokuFrKbn")]
        public string dakokuFrKbn { get; set; }
        [DataMember(Name = "dakokuToTm")]
        public string dakokuToTm { get; set; }
        [DataMember(Name = "dakokuToKbn")]
        public string dakokuToKbn { get; set; }
    }

    [DataContract]
    class KinmuRecordJson
    {
        [DataMember(Name = "kinmuDt")]
        public string kinmuDt = "";

        public DateTime? dakokuFr;
        public DateTime? dakokuTo;
        public DateTime? kinmuFr;
        public DateTime? kinmuTo;

        [DataMember(Name = "dakokuFr")]
        public string dakokuFrString
        {
            get
            {
                return dakokuFrString;
            }
            set
            {
                dakokuFr = FormatDate(value);
            }
        }
        [DataMember(Name = "dakokuTo")]
        public string dakokuToString
        {
            get
            {
                return dakokuToString;
            }
            set
            {
                dakokuTo = FormatDate(value);
            }
        }
        [DataMember(Name = "kinmuFr")]
        public string kinmuFrString
        {
            get
            {
                return kinmuFrString;
            }
            set
            {
                kinmuFr = FormatDate(value);
            }
        }
        [DataMember(Name = "kinmuTo")]
        public string kinmuToString
        {
            get
            {
                return kinmuToString;
            }
            set
            {
                kinmuTo = FormatDate(value);
            }
        }

        private DateTime? FormatDate(string str)
        {
            // �p�[�X
            var array = str.Split(",");
            if (array.Length == 3 && !array.Contains(null) && array[0] != "" && array[1] != "" && array[2] != "")
            {
                array[1] = array[1].Replace(":", "");
                DateControl dc = new DateControl(array[0], array[1], array[2]);
                return dc.Origin;
            }
            return null;
        }

        [DataMember]
        public string? kinmuCd { get; set; }

        [DataMember]
        public string? biko { get; set; }
    }
}
