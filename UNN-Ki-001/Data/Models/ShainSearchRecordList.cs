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
        public int CurrentIndex { get; private set; }

        public ShainSearchRecord Current
        {
            get
            {
                return List[CurrentIndex];
            }
        }

        public ShainSearchRecord Next
        {
            get
            {
                CurrentIndex++;
                return Current;
            }
        }

        public ShainSearchRecord Prev
        {
            get
            {
                CurrentIndex--;
                return Current;
            }
        }
    }
}
