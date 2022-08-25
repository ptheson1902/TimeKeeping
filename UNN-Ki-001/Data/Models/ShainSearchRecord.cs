namespace UNN_Ki_001.Data.Models
{
    public class ShainSearchRecord
    {
        public ShainSearchRecord()
        {
            KigyoCd
                = ShainNo
                = ShainNm
                = KoyokeitaiCd
                = KoyokeitaiNm
                = ShozokuCd
                = ShozokuNm
                = ShokushuCd
                = ShokushuNm
                = "";

        }

        public string KigyoCd { get; set; }
        public string ShainNo { get; set; }
        public string ShainNm { get; set; }
        public string KoyokeitaiCd { get; set; }
        public string KoyokeitaiNm { get; set; }
        public string ShozokuCd { get; set; }
        public string ShozokuNm { get; set; }
        public string ShokushuCd { get; set; }
        public string ShokushuNm { get; set; }
    }
}
