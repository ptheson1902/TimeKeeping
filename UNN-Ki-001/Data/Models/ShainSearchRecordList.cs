namespace UNN_Ki_001.Data.Models
{
    public class ShainSearchRecordList
    {
        public ShainSearchRecordList(List<ShainSearchRecord> list, int index)
        {
            List = list;
            CurrentIndex = index;
            CurrentDate = DateTime.Now;
        }

        public List<ShainSearchRecord> List { get; }

        public int CurrentIndex { get; set; }

        public DateTime CurrentDate { get; set; }

        public ShainSearchRecord GetByIndex(int index)
        {
            var res = List[index];
            CurrentIndex = index;
            return res;
        }

        public ShainSearchRecord Get()
        {
            return List[CurrentIndex];
        }

        public ShainSearchRecordList Next()
        {
            CurrentIndex++;
            if (CurrentIndex >= List.Count)
                CurrentIndex = 0;
            return this;
        }

        public ShainSearchRecordList Prev()
        {
            CurrentIndex--;
            if (CurrentIndex < 0)
                CurrentIndex = List.Count - 1;
            return this;
        }

        public ShainSearchRecordList SetIndex(int index)
        {
            CurrentIndex = index;
            return this;
        }

        public ShainSearchRecordList NextMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
            return this;
        }

        public ShainSearchRecordList PrevMonth()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            return this;
        }

        public ShainSearchRecordList SetMonth(DateTime date)
        {
            CurrentDate = date;
            return this;
        }
    }
}
