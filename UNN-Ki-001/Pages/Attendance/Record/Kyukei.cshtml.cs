using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UNN_Ki_001.Data;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Pages.Attendance.Record
{
    public class KyukeiModel : BasePageModel
    {
        public KyukeiModel(KintaiDbContext kintaiDbContext, UserManager<AppUser> userManager) : base(kintaiDbContext, userManager)
        {
        }

        public JsonResult? OnGet(string kinmuDt = "", string shainNo = "", string kigyoCd = "")
        {
            // 必須入力が欠けている場合
            if(kinmuDt == "" || shainNo == "" || kigyoCd == "")
            {
                return null;
            }

            // 休憩レコードを取得
            List<T_Kyukei> kyukeis = _kintaiDbContext.t_Kyukeis
                .Where(e => e.KinmuDt == kinmuDt && shainNo == e.ShainNo && e.KigyoCd == kigyoCd)
                .ToList();

            // 休憩レコードを整形
            int targetDtInt = int.Parse(kinmuDt);
            List<KyukeiOutputModel> response = new List<KyukeiOutputModel>();
            foreach(var item in kyukeis)
            {
                KyukeiOutputModel kom = new();

                // 開始時間
                if(item.DakokuFrDate != null)
                {
                    // 時刻
                    DateTime dt = (DateTime)item.DakokuFrDate;
                    kom.Start = dt.ToString("HH:mm");

                    // 区分
                    int dtInt = int.Parse(dt.ToString("yyyyMMdd"));
                     kom.StartKbn = (dtInt == targetDtInt) ? 0 : (dtInt > targetDtInt) ? 2 : 1;
                }

                // 終了時間
                if (item.DakokuToDate != null)
                {
                    // 時刻
                    DateTime dt = (DateTime)item.DakokuToDate;
                    kom.End = dt.ToString("HH:mm");

                    // 区分
                    int dtInt = int.Parse(dt.ToString("yyyyMMdd"));
                    kom.EndKbn = (dtInt == targetDtInt) ? 0 : (dtInt > targetDtInt) ? 2 : 1;
                }

                // リストに追加
                response.Add(kom);
            }

            // jsonを作成
            return new JsonResult(response) ;
        }
    }

    public class KyukeiOutputModel
    {
        public int StartKbn { get; set; }
        public int EndKbn { get; set; }
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
    }
}


