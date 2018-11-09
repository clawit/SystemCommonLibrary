using System;
using System.Collections.Generic;

namespace SystemCommonLibrary.Data.Collection.ListCompare
{
    public class ListComparer
    {
        public static List<CompareResultItem<T1>> Compare<T1, T2>(IEnumerable<T1> src, IEnumerable<T2> dst, CompareOption<T1, T2> option)
        {
            //返回结果集
            var srcList = new List<CompareResultItem<T1>>();

            //先构造内部用于比较的数据集
            Dictionary<object, CompareItem<T1, T2>> srcCollection = new Dictionary<object, CompareItem<T1, T2>>();
            foreach (var item in src)
            {
                if (item != null)
                {
                    var key = option.GetSrcId(item);
                    var compareItem = new CompareSrcItem<T1, T2>(key, item, option);
                    srcCollection.Add(key, compareItem);
                }
            }
            Dictionary<object, CompareItem<T1, T2>> dstCollection = new Dictionary<object, CompareItem<T1, T2>>();
            foreach (var item in dst)
            {
                if (item != null)
                {
                    var key = option.GetDstId(item);
                    var compareItem = new CompareDstItem<T1, T2>(key, item, option);
                    dstCollection.Add(key, compareItem);
                }
            }

            //从T1的集合中查找T2的集合
            //如果没找到，说明此数据从T2中删除了
            //如果找到了，那么就此对比具体的成员值
            //（每次找到后，要从T2集合移除，最后剩下的就全是新增的）
            foreach (var item in srcCollection)
            {
                if (dstCollection.ContainsKey(item.Key))
                {
                    CompareItem<T1, T2> compareItem2 = dstCollection[item.Key];
                    if (item.Value.IsDeleted()
                        || item.Value.IsDifferent(compareItem2))
                    {
                        //目标数据集中被更新
                        option.ModifyItem((T1)item.Value.Item, (T2)compareItem2.Item);
                        srcList.Add(new CompareResultItem<T1>()
                        {
                            Item = (T1)item.Value.Item,
                            Result = CompareResult.Modify
                        });
                    }

                    dstCollection.Remove(item.Key);
                }
                else
                {
                    option.RemoveItem((T1)item.Value.Item);
                    //目标数据集中被删除
                    srcList.Add(new CompareResultItem<T1>() { Item = (T1)item.Value.Item, Result = CompareResult.Delete });
                }
            }
            //最后剩下的就全是新增的（每次找到后，会从T2集合移除）
            foreach (var item in dstCollection)
            {
                T1 t1 = option.CreateNewItem((T2)item.Value.Item);
                //目标数据集中是新增的 
                srcList.Add(new CompareResultItem<T1>() { Item = t1, Result = CompareResult.New });
            }
            dstCollection.Clear();

            return srcList;
        }

    }
}
