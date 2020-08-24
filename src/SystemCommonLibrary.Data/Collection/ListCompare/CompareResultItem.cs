using System;

namespace SystemCommonLibrary.Data.Collection.ListCompare
{
    public enum CompareResult
    {
        New,
        Modify,
        Delete
    }

    public class CompareResultItem<T>
    {
        public T Item { get; set; }

        public CompareResult Result { get; set; }
    }

}
