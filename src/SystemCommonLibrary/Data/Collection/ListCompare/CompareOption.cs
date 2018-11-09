using System;
using System.Collections.Generic;

namespace SystemCommonLibrary.Data.Collection.ListCompare
{
    public delegate T1 CreateNewItem<T1, T2>(T2 item2);
    public delegate void ModifyItem<T1, T2>(T1 item1, T2 item2);
    public delegate void RemoveItem<T1>(T1 item1);

    public delegate object GetSrcId<T1>(T1 src);
    public delegate object GetDstId<T2>(T2 src);


    public class CompareOption<T1, T2>
    {
        public string ColumnNameOfRemove { get; }

        public Dictionary<string, string> ColumnsToCompare { get; }

        public CreateNewItem<T1, T2> CreateNewItem { get; }
        public ModifyItem<T1, T2> ModifyItem { get; }
        public RemoveItem<T1> RemoveItem { get; }

        public GetSrcId<T1> GetSrcId { get; }
        public GetDstId<T2> GetDstId { get; }

        public CompareOption(Dictionary<string, string> columnsToCompare,
            CreateNewItem<T1, T2> createNewItem, ModifyItem<T1, T2> modifyItem, RemoveItem<T1> removeItem,
            GetSrcId<T1> getSrcId, GetDstId<T2> getDstId,
            string columnNameOfRemove = "IsDeleted")
        {
            if (columnsToCompare == null || columnsToCompare.Count == 0)
            {
                throw new Exception("No compare columns are specified.");
            }

            this.ColumnNameOfRemove = columnNameOfRemove;
            this.ColumnsToCompare = columnsToCompare;

            this.CreateNewItem = new CreateNewItem<T1, T2>(createNewItem);
            this.ModifyItem = new ModifyItem<T1, T2>(modifyItem);
            this.RemoveItem = new RemoveItem<T1>(removeItem);

            this.GetSrcId = new GetSrcId<T1>(getSrcId);
            this.GetDstId = new GetDstId<T2>(getDstId);
        }
    }
}
