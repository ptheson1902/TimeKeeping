using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public string TargetShainNo = "";
        public string TargetKigyoCd = "";
        public List<string[]> MKinmuInfoList = new();
        public ShainSearchRecordList? ShainList;
        public ShainSearchRecord? TargetShain;


        public IndexModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {

        }

        [ResponseCache(Duration = 30)]
        public IActionResult OnGet()
        {
            Init();
            return Page();
        }

        public IActionResult OnPost(string command = "")
        {
            Init();
            // セッションに値が存在しない場合は社員検索へリダイレクトさせる
            if (ShainList == null || TargetShain == null)
                return RedirectToPage("/Attendance/Record/Search");

            // 操作コマンドの処理
            if(ShainList != null)
            {
                switch (command)
                {
                    case "nextMonth":
                        TargetShain.GetNextMonth();
                        break;
                    case "prevMonth":
                        TargetShain.GetPrevMonth();
                        break;
                    case "nextShain":
                        ShainList.GetNext();
                        break;
                    case "prevShain":
                        ShainList.GetPrev();
                        break;
                    default:
                        break;
                }

                // 変更後のオブジェクトをセッションへ再配置
                HttpContext.Session.SetObj(Constants.SEARCH_RECORD_LIST, ShainList);
            }

            return RedirectToPage("/Attendance/Record/Index");
        }

        private void Init()
        {
            // セッションから検索対象を取得
            ShainList = HttpContext.Session.GetObject<ShainSearchRecordList>(Constants.SEARCH_RECORD_LIST);

            // 参照対象が確認できなければここで終了
            if (ShainList == null)
                return;

            // カレントインデックスの社員を取り出す
            TargetShain = ShainList.GetCurrent();

            // 勤務内容のリストを取得する
            MKinmuInfoList.Add(new string[] { "", "" }); // ←社員検索にて企業コードのNotNullableが保証されている(シリアライズする際にコンストラクタの都合でNullableになっている)
            var mKinmList = _kintaiDbContext.m_kinmus
                .Where(e => e.KigyoCd!.Equals(TargetShain.KigyoCd) && e.ValidFlg != null && e.ValidFlg.Equals("1"))
                .ToList();
            foreach (var item in mKinmList)
            {
                MKinmuInfoList.Add(new string[] { item.KinmuCd, item.KinmuNm ?? "名称未設定" });
            }


            // 閲覧月のデータを取得
            DateTime currentMonth = TargetShain.GetCurrentDate();
            // 企業コードで設定をSELECT
            var setting = _kintaiDbContext.mSettings
                .Where(e => e.KigyoCd.Equals(TargetShain.KigyoCd))
                .FirstOrDefault();
            // 設定から締め日を抽出（初期値: 99(末日))
            int shimebi = (setting == null || setting.ShimeDt == null) ? 99 : (int)setting.ShimeDt;

            // 締め日翌日から締め日までのKinmuDt一覧を取得
            DayList calender = new DayList(currentMonth, shimebi);
            var kinmuDtList = calender.KinmuDtList;

            // 勤務データを取り込み
            var tempList = _kintaiDbContext.t_kinmus
                .Include(e => e.MKinmu)
                .Where(e => e.KigyoCd!.Equals(TargetShain.KigyoCd)
                    && e.ShainNo!.Equals(TargetShain.ShainNo)
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
                    DataList.Add(data);
                    continue;
                }

                // 勤務コード
                if (kinmu.KinmuCd != null)
                    data.KinmuCd = kinmu.KinmuCd;

                // 勤務予定
                if (kinmu.MKinmu != null && kinmu.MKinmu.KinmuNm != null)
                    data.Yote = kinmu.MKinmu.KinmuNm;

                // 時刻フォーマットを宣言
                string timeFormat = "HH:mm";

                // 時刻
                if (kinmu.DakokuFrDate != null)
                    data.DakokuStart = ((DateTime)kinmu.DakokuFrDate).ToString(timeFormat);
                if (kinmu.DakokuToDate != null)
                    data.DakokuEnd = ((DateTime)kinmu.DakokuToDate).ToString(timeFormat);
                if (kinmu.KinmuFrDate != null)
                    data.KinmuStart = ((DateTime)kinmu.KinmuFrDate).ToString(timeFormat);
                if (kinmu.KinmuToDate != null)
                    data.KinmuEnd = ((DateTime)kinmu.KinmuToDate).ToString(timeFormat);

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
                DataList.Add(data);
            }
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
}
