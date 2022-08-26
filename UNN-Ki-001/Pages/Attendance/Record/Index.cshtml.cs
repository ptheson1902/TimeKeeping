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
        // 時刻フォーマットを宣言
        public static readonly string TIME_FORMAT = "HH:mm";

        public List<Kinmuhyo> DataList = new();
        public List<string[]> MKinmuInfoList = new();
        public ShainSearchRecord Target = new();
        public ShainSearchRecordList? TargetList = null;
        public string TargetListJson = "";

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {

        }

        /// <summary>
        /// セッションからターゲットを選出して勤務表を表示します。
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            // 表示対象をセッションから確定します。
            TargetListJson = HttpContext.Session.GetString(Constants.SEARCH_RECORD_LIST) ?? "";
            TargetList = JsonConvert.DeserializeObject<ShainSearchRecordList>(TargetListJson);
            // もし存在しなければ社員検索へ飛ばして作成してもらいます。
            if(TargetList == null)
                return RedirectToPage("/Attendance/Record/Search");

            // 表示内容を生成します。
            Target = TargetList.Get();
            var mKinmuList = _kintaiDbContext.m_kinmus
                .Where(e => e.KigyoCd == Target.KigyoCd)
                .ToList();
            foreach(var item in mKinmuList)
            {
                MKinmuInfoList.Add(new string[] { item.KigyoCd, item.KinmuCd, item.KinmuNm ?? "名称未設定" });
            }
            DataList = CreateData(Target, TargetList.CurrentDate);

            return Page();
        }

        /// <summary>
        /// パラメーターを受け取って画面遷移します。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IActionResult OnPost(string targetListJson = "", string command = "", string json = "")
        {

            // パラメーターを受け取ります。
            TargetListJson = targetListJson;

            // ターゲットリストをデシリアライズします。
            // TODO: デシリアライズ失敗時のテストをしてください。
            TargetList = JsonConvert.DeserializeObject<ShainSearchRecordList>(targetListJson);
            // もし存在しなければ社員検索へ飛ばします。
            if (TargetList == null)
                return RedirectToPage("/Attendance/Record/Search");
            Target = TargetList.Get();

            // jsonが添付されていればDBに変更を適用します。
            // デシリアライズする
            var ChangeList = JsonConvert.DeserializeObject<Dictionary<string, KinmuRecordJson>>(json);
            if (ChangeList != null)
            {
                foreach (var item in ChangeList)
                {
                    var kinmuDt = item.Key;
                    var value = item.Value;

                    // もしも既にレコードが存在すれば取り寄せる
                    var kinmu = _kintaiDbContext.t_kinmus
                        .Where(e => e.KigyoCd == Target.KigyoCd
                            && e.ShainNo == Target.ShainNo
                            && e.KinmuDt == value.kinmuDt)
                        .FirstOrDefault();
                    // そうでなければ作ります
                    if(kinmu == null)
                    {
                        kinmu = new T_Kinmu();
                        kinmu.KigyoCd = Target.KigyoCd;
                        kinmu.KinmuDt = value.kinmuDt;
                        kinmu.ShainNo = Target.ShainNo;

                        _kintaiDbContext.Add(kinmu);
                    }

                    // パラメーターに従い値を変更
                    kinmu.DakokuFrDate = value.dakokuFr;
                    kinmu.DakokuToDate = value.dakokuTo;
                    kinmu.KinmuFrDate = value.kinmuFr;
                    kinmu.KinmuToDate = value.kinmuTo;
                }



                // 変更を適用
                _kintaiDbContext.SaveChanges();
            }

            // コマンドを適用して表示を変更します
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
            }

            // 表示内容を生成します。
            DataList = CreateData(Target, TargetList.CurrentDate);
            var mKinmuList = _kintaiDbContext.m_kinmus
            .Where(e => e.KigyoCd == Target.KigyoCd)
            .ToList();
            foreach (var item in mKinmuList)
            {
                MKinmuInfoList.Add(new string[] { item.KigyoCd, item.KinmuCd, item.KinmuNm ?? "名称未設定" });
            }
            Target = TargetList.Get();

            // 再びシリアライズしてビューに渡します
            // TODO: シリアライズ失敗時のテストをしてください。
            TargetListJson = JsonConvert.SerializeObject(TargetList);

            return Page();
        }

        private List<Kinmuhyo> CreateData(ShainSearchRecord target, DateTime month)
        {
            // 結果格納用の変数
            List<Kinmuhyo> result = new();

            // 企業コードで設定をSELECT
            var setting = _kintaiDbContext.mSettings
                .Where(e => e.KigyoCd.Equals(target.KigyoCd))
                .FirstOrDefault();
            // 設定から締め日を抽出（初期値: 99(末日))
            int shimebi = (setting == null || setting.ShimeDt == null) ? 99 : (int)setting.ShimeDt;

            // 締め日翌日から締め日までのKinmuDt一覧を取得
            DayList calender = new(month, shimebi);
            var kinmuDtList = calender.KinmuDtList;

            // 勤務データを取り込み
            var tempList = _kintaiDbContext.t_kinmus
                .Include(e => e.MKinmu)
                .Where(e => e.KigyoCd!.Equals(target.KigyoCd)
                    && e.ShainNo!.Equals(target.ShainNo)
                    && kinmuDtList.Contains(e.KinmuDt!))
                .OrderBy(e => e.KinmuDt)
                .ToList();

            // 存在しないデータも埋めて出力用の整形済みデータを作成する
            CultureInfo Japanese = new CultureInfo("ja-JP");
            foreach (var kinmuDt in kinmuDtList)
            {
                var data = new Kinmuhyo(kinmuDt);
                var day = DateTime.ParseExact(kinmuDt, "yyyyMMdd", null); // 変数kinmuDtがyyyyMMddのフォーマットであることはDayListクラスによって保証されている
                data.Day = day.ToString("MM月dd日(ddd)", Japanese);

                // 勤務レコードを取得
                var kinmu = tempList
                    .Where(e => e.KinmuDt!.Equals(kinmuDt))
                    .FirstOrDefault();

                // nullならcontinue(初期値はコンストラクタで挿入済み）
                if (kinmu == null)
                {
                    result.Add(data);
                    continue;
                }

                // 勤務コード
                if (kinmu.KinmuCd != null)
                    data.KinmuCd = kinmu.KinmuCd;

                // 勤務予定
                if (kinmu.MKinmu != null && kinmu.MKinmu.KinmuNm != null)
                    data.Yote = kinmu.MKinmu.KinmuNm;

                // 時刻
                if (kinmu.DakokuFrDate != null)
                    data.DakokuStart = ((DateTime)kinmu.DakokuFrDate).ToString(TIME_FORMAT);
                if (kinmu.DakokuToDate != null)
                    data.DakokuEnd = ((DateTime)kinmu.DakokuToDate).ToString(TIME_FORMAT);
                if (kinmu.KinmuFrDate != null)
                    data.KinmuStart = ((DateTime)kinmu.KinmuFrDate).ToString(TIME_FORMAT);
                if (kinmu.KinmuToDate != null)
                    data.KinmuEnd = ((DateTime)kinmu.KinmuToDate).ToString(TIME_FORMAT);

                // 休憩
                if (kinmu.Kyukei != null)
                    data.Kyukei = MinutesToString((int)kinmu.Kyukei);

                //総労働
                if (kinmu.Sorodo != null)
                    data.Sorodo = MinutesToString((int)kinmu.Sorodo);

                // 控除
                if (kinmu.Kojo != null)
                    data.Kojo = MinutesToString((int)kinmu.Kojo);

                // 備考
                if (kinmu.Biko != null)
                    data.Biko = kinmu.Biko;

                // 作成完了・リストに追加
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
            Yote = "未設定";
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
    /// 日付を指定するとその月のカレンダーを生成するオブジェクトです。
    /// </summary>
    class DayList
    {
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;
        public readonly int Count;
        public readonly List<string> KinmuDtList; 

        public DayList(DateTime date, int shimebi)
        {
            // 締め日に99を指定された場合は、月末を意味するので割り込み処理
            if (shimebi == 99)
            {
                shimebi = 0;
            }
            // C#のDateTimeは0以下の日付に対応できないため、その場合は後から差分を減算することで対応します。
            // よって、０以下の場合は1との差を計測します
            int mainasu = 0;
            if (shimebi < 1)
            {
                mainasu = shimebi - 1;
                shimebi = 1;
            }

            // 開始日を計算
            var startYear = date.Year;
            var startMonth = date.Month;


            // 締め日を計算
            var endDate = date.AddMonths(1);
            var endYear = endDate.Year;
            var endMonth = endDate.Month;

            // 日付型を作成
            StartDate = new DateTime(startYear, startMonth, shimebi);
            StartDate = StartDate.AddDays(1 + mainasu);
            EndDate = new DateTime(endYear, endMonth, shimebi);
            EndDate = EndDate.AddDays(mainasu);

            // 日数を取得して格納
            Count = (EndDate - StartDate).Days + 1;

            // カレンダー形式で勤務記録を取得
            KinmuDtList = GetKinmuDtList(StartDate, Count);
        }

        public static List<string> GetKinmuDtList(DateTime startDay, int length)
        {
            // kinmuDtのリストを作成
            List<String> kinmuDtList = new List<string>();
            for(int i=0; i<length; i++)
            {
                kinmuDtList.Add(startDay.AddDays(i).ToString("yyyyMMdd"));
            }

            return kinmuDtList;
        }
    }

    [DataContract]
    class KinmuRecordJson
    {
        [DataMember(Name = "kinmuDt")]
        public string kinmuDt = "";

        public DateTime dakokuFr;
        public DateTime dakokuTo;
        public DateTime kinmuFr;
        public DateTime kinmuTo;

        [DataMember(Name = "dakokuFr")]
        public string dakokuFrString
        {
            get
            {
                return dakokuFrString;
            }
            set
            {
                // パース
                var array = value.Split(",");
                var tm = array[0];
                var kbn = array[1];


                try
                {
                    // tmを変換
                    var res = DateTime.ParseExact(tm, "yyyyMMdd_" + IndexModel.TIME_FORMAT, null);

                    // kbnを適用
                    if (kbn == "1")
                    {
                        res = res.AddDays(1);
                    }
                    else if (kbn == "-1")
                    {
                        res = res.AddDays(-1);
                    }

                    dakokuFr = res;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                
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
                // パース
                var array = value.Split(",");
                var tm = array[0];
                var kbn = array[1];

                try
                {
                    // tmを変換
                    var res = DateTime.ParseExact(tm, "yyyyMMdd_" + IndexModel.TIME_FORMAT, null);

                    // kbnを適用
                    if (kbn == "1")
                    {
                        res = res.AddDays(1);
                    }
                    else if (kbn == "-1")
                    {
                        res = res.AddDays(-1);
                    }

                    dakokuTo = res;
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                
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
                // パース
                var array = value.Split(",");
                var tm = array[0];
                var kbn = array[1];



                try
                {

                    // tmを変換
                    var res = DateTime.ParseExact(tm, "yyyyMMdd_" + IndexModel.TIME_FORMAT, null);
                    // kbnを適用
                    if (kbn == "1")
                    {
                        res = res.AddDays(1);
                    }
                    else if (kbn == "-1")
                    {
                        res = res.AddDays(-1);
                    }

                    kinmuFr = res;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                
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
                // パース
                var array = value.Split(",");
                var tm = array[0];
                var kbn = array[1];

                

                try
                {
                    // tmを変換
                    var res = DateTime.ParseExact(tm, "yyyyMMdd_" + IndexModel.TIME_FORMAT, null);
                    // kbnを適用
                    if (kbn == "1")
                    {
                        res = res.AddDays(1);
                    }
                    else if (kbn == "-1")
                    {
                        res = res.AddDays(-1);
                    }

                    kinmuTo = res;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                
            }
        }

    }
}
