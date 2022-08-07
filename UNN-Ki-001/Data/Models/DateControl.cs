namespace UNN_Ki_001.Data.Models
{
    public class DateControl
    {
        /// <summary>
        /// DateTimeを今プロジェクトで用いる型に変換する。
        /// </summary>
        /// <param name="dateTime">日付</param>
        public DateControl(DateTime dateTime)
        {
            // 文字列型に変換して格納
            string[] tempData = dateTime.ToString("yyyyMMdd,HHmm").Split(",");
            Date = tempData[0]!;
            Time = tempData[1]!;

            TimeInt = int.Parse(Time);
            DateInt = int.Parse(Date);
        }

        /// <summary>
        /// DateControlによって丸め処理されたDateControl。
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        private DateControl(string date, string time)
        {
            Date = date;
            Time = time;
        }

        /// <summary>
        /// 丸め単位時間と丸め区分を指定して丸め処理を実行しインスタンスを作成する。
        /// パラメーターが不正であった場合Parseに失敗し例外がスローされる可能性があります。
        /// </summary>
        /// <param name="marumeTm"></param>
        /// <param name="marumeKbn"></param>
        /// <returns>丸め処理実行済みのDateControl</returns>
        public DateControl MarumeProcess(string marumeTm, string marumeKbn)
        {
            if (marumeKbn.Equals("0") || marumeTm == null || marumeKbn == null)
            {
                // 処理なし
                return this;
            }
            else
            {
                DateTime dateTime = DateTime.ParseExact(Date + Time, "yyyyMMddHHmm", null);
                TimeSpan ts = TimeSpan.FromMinutes(int.Parse(marumeTm));

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

                string[] resultTemp = dateTime.ToString("yyyyMMdd,HHmm").Split(",");
                DateControl result = new DateControl(resultTemp[0], resultTemp[1]);
                return result;
            }
        }

        /// <summary>
        /// DateをInt型にパースしたもの。
        /// </summary>
        public int DateInt { get; }

        /// <summary>
        /// TimeをInt型にパースしたもの。
        /// </summary>
        public int TimeInt { get; }

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
