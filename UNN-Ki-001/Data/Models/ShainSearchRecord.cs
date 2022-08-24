namespace UNN_Ki_001.Data.Models
{
    public class ShainSearchRecord
    {
        public int CurrentIndex { get; set; }

        public DateTime TargetDate = DateTime.Now;

        public DateTime GetCurrentDate()
        {
            return TargetDate;
        }

        public DateTime GetNextMonth()
        {
            TargetDate = TargetDate.AddMonths(1);
            return TargetDate;
        }

        public DateTime GetPrevMonth()
        {
            TargetDate = TargetDate.AddMonths(-1);
            return TargetDate;
        }
        public string? KigyoCd { get; set; }
        public string? ShainNo { get; set; }
        public string? ShainNm { get; set; }
        public string? KoyokeitaiCd { get; set; }
        public string? KoyokeitaiNm { get; set; }
        public string? ShozokuCd { get; set; }
        public string? ShozokuNm { get; set; }
        public string? ShokushuCd { get; set; }
        public string? ShokushuNm { get; set; }
    }
}
