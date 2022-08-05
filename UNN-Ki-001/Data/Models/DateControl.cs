namespace UNN_Ki_001.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DateControl
    {
        public DateControl(DateTime dateTime)
        {
            Origin = dateTime;

            // 万が一、フォーマット後のデータがnullであった場合の初期値を入力する
            const string errorString = "Error";
            Date ??= errorString;
            Time ??= errorString;
        }

        /// <summary>
        /// フォーマット対象のオリジンデータです。
        /// </summary>
        public DateTime Origin
        {
            get
            {
                return Origin;
            }
            set
            {
                // DateTime型を格納
                Origin = value;

                // 文字列型に変換して格納
                string[] tempData = value.ToString("yyyyMMdd,HHmm").Split(",");
                Date = tempData[0]!;
                Time = tempData[1]!;
            }
        }
        /// <summary>
        /// オリジンデータを"yyyyMMdd"の文字列型へフォーマットしたデータです。
        /// </summary>
        public string Date { get; private set; }

        /// <summary>
        /// オリジンデータを"HHmm"の文字列型へフォーマットしたデータです。
        /// </summary>
        public string Time { get; private set; }
    }
}
