namespace UNN_Ki_001.Data.Models
{
    public class ShainSearchOption
    {
        public ShainSearchOption(List<M_Shain> resultList)
        {
            if(resultList.Count == 0)
            {
                throw new Exception("RecordSearchOptionにサイズが０のリストが渡されました。");
            }

            this.ResultList = resultList;
            this.CurrentIndex = 0;
        }

        public List<M_Shain> ResultList { get; }
        public int CurrentIndex { get; private set; }

        public M_Shain CurrentElem
        {
            get
            {
                return ResultList[CurrentIndex];
            }
        }

        public M_Shain NextElem
        {
            get
            {
                CurrentIndex++;
                return CurrentElem;
            }
        }

        public M_Shain PrevElem
        {
            get
            {
                CurrentIndex--;
                return CurrentElem;
            }
        }
    }
}
