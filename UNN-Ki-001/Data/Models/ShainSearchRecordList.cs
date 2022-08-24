namespace UNN_Ki_001.Data.Models
{
    public class ShainSearchRecordList
    {
        public ShainSearchRecordList(List<ShainSearchRecord> list, int index)
        {
            this.List = list;
            this.CurrentIndex = index;
        }

        public List<ShainSearchRecord> List { get; }

        public int CurrentIndex { get; set; }

        public ShainSearchRecord GetCurrent()
        {
            return List[CurrentIndex];
        }

        public ShainSearchRecord GetNext()
        {
            CurrentIndex++;
            if (CurrentIndex >= List.Count)
                CurrentIndex = 0;
            return GetCurrent();
        }

        public ShainSearchRecord GetPrev()
        {
            CurrentIndex--;
            if (CurrentIndex < 0)
                CurrentIndex = List.Count - 1;
            return GetCurrent();
        }
    }
}
