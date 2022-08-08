namespace UNN_Ki_001.Data.Models
{
    public class DateControl
    {
        /// <summary>
        /// DateTimeを今プロジェクトで用いる型に変換する。
        /// </summary>
        /// <param name="dateTime">日付</param>
        public DateControl(DateTime dateTime, string kbn = "0")
        {
            // 区分の適用
            if(kbn != null)
            {
                if (kbn.Equals("1"))
                {
                    dateTime = dateTime.AddDays(-1);
                }
                else if (kbn.Equals("2"))
                {
                    dateTime = dateTime.AddDays(1);
                }
            }

            // 文字列型に変換して格納
            string[] tempData = dateTime.ToString("yyyyMMdd,HHmm").Split(",");
            Date = tempData[0]!;
            Time = tempData[1]!;

            Origin = dateTime;
        }

        /// <summary>
        /// DBの日時からインスタンスを作成する。キャストに失敗すると例外が発生します。
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        public DateControl(string date, string time = "0000", string kbn = "0")
            : this(DateTime.ParseExact(date + time, "yyyyMMddHHmm", null), kbn) { }

        public DateTime Origin { get; }

        /// <summary>
        /// 丸め単位時間と丸め区分を指定して丸め処理を実行しインスタンスを作成する。
        /// パラメーターが不正であった場合Parseに失敗し例外がスローされる可能性があります。
        /// </summary>
        /// <param name="marumeTm"></param>
        /// <param name="marumeKbn"></param>
        /// <returns>丸め処理実行済みのDateControl</returns>
        public DateControl MarumeProcess(int? marumeTm, string? marumeKbn)
        {
            if (marumeKbn == null || marumeKbn.Equals("0") || marumeTm == null || marumeTm == 0)
            {
                // 処理なし
                return this;
            }
            else
            {
                // 計算用の型へキャストする
                DateTime dateTime = DateTime.ParseExact(Date + Time, "yyyyMMddHHmm", null);
                TimeSpan ts = TimeSpan.FromMinutes((double)marumeTm);

                if (marumeKbn.Equals("1"))
                {
                    // 切り上げ処理
                    dateTime = new DateTime(((dateTime.Ticks + ts.Ticks - 1) / ts.Ticks) * ts.Ticks, dateTime.Kind);
                }
                else
                {
                    // 切り下げ処理
                    dateTime = new DateTime((((dateTime.Ticks + ts.Ticks) / ts.Ticks) - 1) * ts.Ticks, dateTime.Kind);
                }

                DateControl result = new DateControl(dateTime);
                return result;
            }
        }

        /// <summary>
        /// オリジンデータを"yyyyMMdd"の文字列型へフォーマットしたデータです。
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// オリジンデータを"HHmm"の文字列型へフォーマットしたデータです。
        /// </summary>
        public string Time { get; }
    }
}
