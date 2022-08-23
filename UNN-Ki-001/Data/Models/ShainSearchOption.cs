﻿namespace UNN_Ki_001.Data.Models
{
    public class ShainSearchRecordList
    {
        public ShainSearchRecordList(List<ShainSearchRecord> list, int index)
        {
            if(list.Count == 0)
            {
                throw new Exception("RecordSearchOptionにサイズが０のリストが渡されました。");
            }

            this.ResultList = list;
            this.CurrentIndex = index;
        }

        public List<ShainSearchRecord> ResultList { get; }
        public int CurrentIndex { get; private set; }

        public ShainSearchRecord CurrentElem
        {
            get
            {
                return ResultList[CurrentIndex];
            }
        }

        public ShainSearchRecord NextElem
        {
            get
            {
                CurrentIndex++;
                return CurrentElem;
            }
        }

        public ShainSearchRecord PrevElem
        {
            get
            {
                CurrentIndex--;
                return CurrentElem;
            }
        }
    }
}
