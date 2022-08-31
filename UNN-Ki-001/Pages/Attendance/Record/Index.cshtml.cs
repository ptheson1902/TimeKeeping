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
        public string ShoteiTimes = "";
        public string SorodoTimes = "";
/*      public int? ShoteiDays = 0;
        public int? YasumiDays = 0;
        public int? YukyuDays = 0;*/

        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {

        }

        /// <summary>
        /// セッションからターゲットを選出して勤務表を表示します。
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Init();
        }

        /// <summary>
        /// パラメーターを受け取って画面遷移します。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IActionResult OnPost(string targetListJson = "", string json = "", string kyukeiJson = "", string command = "")
        {
            // 管理者じゃなければパラメーターを上書きする
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
                // 表示対象をセッションから確定します。
                TargetListJson = HttpContext.Session.GetString(Constants.SEARCH_RECORD_LIST) ?? "";
            }
            else
            {
                // 表示対象をパラメーターから取得します
                TargetListJson = targetListJson;
            }
            // デシリアライズ実行
            TargetList = JsonConvert.DeserializeObject<ShainSearchRecordList>(TargetListJson);

            // もし存在しなければ社員検索へ飛ばして作成してもらいます。
            if (TargetList == null)
                return RedirectToPage("/Attendance/Record/Search");

            // コマンドを処理、ターゲットの情報を確定します。
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

            // 休憩jsonが指定されていればjsonの処理
            if(kyukeiJson != "")
            {


                // デシリアライズする
                var ChangeList = JsonConvert.DeserializeObject<KyukeiRecordJsonList>(kyukeiJson);

                if(ChangeList != null)
                {
                    // 日付を取得
                    var kinmuDt = ChangeList.kinmuDt;
                    // まず既存の休憩をすべて削除する
                    var kyukeis = _kintaiDbContext.t_Kyukeis
                        .Where(e => e.KigyoCd == Target.KigyoCd
                            && e.ShainNo == Target.ShainNo
                            && e.KinmuDt == kinmuDt)
                        .ToList();
                    _kintaiDbContext.RemoveRange(kyukeis);

                    // 勤務レコードを用意する
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
                        // 集計の初期化処理
                        if((kinmu.KinmuFrDate != null && kinmu.KinmuToDate != null))
                        {
                            kinmu.ClearInfo();
                        }
                    }

                    // jsonをもとに新規作成して追加
                    int count = 0;
                    foreach (var item in ChangeList.list)
                    {
                        // 日時を変換
                        DateControl frDc = new DateControl(kinmuDt, item.dakokuFrTm.Replace(":", ""), item.dakokuFrKbn);
                        DateControl toDc = new DateControl(kinmuDt, item.dakokuToTm.Replace(":", ""), item.dakokuToKbn);
                        // 休憩レコードを作成
                        T_Kyukei kyukei = new(Target.KigyoCd, Target.ShainNo, kinmuDt, count++, (T_Kinmu)kinmu);
                        kyukei.DakokuFrDate = frDc.Origin;
                        kyukei.DakokuToDate = toDc.Origin;

                        // DBに追加
                        _kintaiDbContext.Add(kyukei);

                    }
                    _kintaiDbContext.SaveChanges();
                }
            }

            // jsonが指定されていればDBを変更処理する
            if (json != "")
            {
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
                                && e.KinmuDt == kinmuDt)
                            .FirstOrDefault();
                        // そうでなければ作ります
                        if (kinmu == null)
                        {
                            kinmu = new T_Kinmu();
                            kinmu.KigyoCd = Target.KigyoCd;
                            kinmu.KinmuDt = kinmuDt;
                            kinmu.ShainNo = Target.ShainNo;

                            _kintaiDbContext.Add(kinmu);
                        } else
                        {
                            
                            // 集計の初期化を適宜行います
                            if((value.kinmuCd != "" && kinmu.KinmuCd != value.kinmuCd) || (value.kinmuFr == null || value.kinmuTo == null))
                            {
                                kinmu.ClearInfo();
                            }
                        }


                        // パラメーターに従い値を変更
                        //kinmu.DakokuFrDate = value.dakokuFr; // 打刻のみ変更するとReloadableによって勤務記録が作成されるのが
                        //kinmu.DakokuToDate = value.dakokuTo; // 一見するとバグなので、いったん変更できないようにしています。
                        kinmu.KinmuFrDate = value.kinmuFr;
                        kinmu.KinmuToDate = value.kinmuTo;
                        kinmu.KinmuCd = value.kinmuCd;
                        kinmu.Biko = value.biko;
                    }   

                    // 変更を適用
                    _kintaiDbContext.SaveChanges();
                }
            }

            // View表示する勤務表の内容を生成します。
            DataList = CreateData(Target, TargetList.CurrentDate);
            

            // それに使う勤務マスタの名称を取得します。
            var mKinmuList = _kintaiDbContext.m_kinmus
                .Where(e => e.KigyoCd == Target.KigyoCd)
                .ToList();
            foreach (var item in mKinmuList)
            {
                MKinmuInfoList.Add(new string[] { item.KigyoCd, item.KinmuCd, item.KinmuNm ?? "名称未設定" });
            }

            // 再びシリアライズしてビューに渡します
            // TODO: シリアライズ失敗時のテストをしてください。
            TargetListJson = JsonConvert.SerializeObject(TargetList);
            // ついでにセッションも上書きします。
            HttpContext.Session.SetString(Constants.SEARCH_RECORD_LIST, TargetListJson);

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
            int? shoteiTimes = 0;
            int? sorodoTimes = 0;
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
                int standardYM = int.Parse(kinmuDt);

                if (kinmu.DakokuFrDate != null)
                {
                    var datetime = (DateTime)kinmu.DakokuFrDate;
                    data.DakokuStart = datetime.ToString(TIME_FORMAT);
                    // 区分の計算
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.DakokuFrKbn = (targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }
                if (kinmu.DakokuToDate != null)
                {
                    var datetime = (DateTime)kinmu.DakokuToDate;
                    data.DakokuEnd = datetime.ToString(TIME_FORMAT);
                    // 区分の計算
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.DakokuToKbn = (targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }
                if (kinmu.KinmuFrDate != null)
                {
                    var datetime = (DateTime)kinmu.KinmuFrDate;

                    data.KinmuStart = datetime.ToString(TIME_FORMAT);
                    // 区分の計算
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.KinmuFrKbn = ( targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }
                if (kinmu.KinmuToDate != null)
                {
                    var datetime = (DateTime)kinmu.KinmuToDate;
                    data.KinmuEnd = datetime.ToString(TIME_FORMAT);
                    // 区分の計算
                    int targetYM = int.Parse(datetime.ToString("yyyyMMdd"));
                    data.KinmuToKbn = (targetYM == standardYM) ? 0 : (targetYM > standardYM) ? 1 : 2;
                }

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
                // 所定時間
                if (kinmu.Shotei != null && kinmu.KinmuFrDate != null && kinmu.KinmuToDate != null)
                    shoteiTimes += kinmu.Shotei;
                // 総労働
                if (kinmu.Sorodo != null && kinmu.KinmuFrDate != null && kinmu.KinmuToDate != null)
                    sorodoTimes += kinmu.Sorodo;
                /*// 所定日数
                if (kinmu.MKinmu != null)
                {
                    if(kinmu.MKinmu.KinmuBunrui ==)
                }
                else
                {
                    YasumiDays++;
                }*/
                // 作成完了・リストに追加
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
            // パース
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
