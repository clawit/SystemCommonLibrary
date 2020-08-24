using System;
using System.Collections.Generic;

namespace SystemCommonLibrary.Data.Collection.ListCompare
{
    public class CompareItem<T1, T2>
    {
        public Dictionary<string, object> ColumnsValue { get; set; } = new Dictionary<string, object>();

        public object Key { get; set; }

        public object Item { get; }

        public CompareOption<T1, T2> Option { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="option"></param>
        /// <param name="isSrc">标识此数据是T1还是T2的数据</param>
        protected CompareItem(object key, object item, CompareOption<T1, T2> option, bool isSrc = false)
        {
            this.Item = item;
            this.Key = key;
            this.Option = option;
            var properties = item.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (isSrc)
                {
                    //
                    if (Option.ColumnsToCompare.ContainsKey(property.Name)
                        || property.Name == Option.ColumnNameOfRemove)
                    {
                        ColumnsValue.Add(property.Name, property.GetValue(item));
                    }
                }
                else
                {
                    if (Option.ColumnsToCompare.ContainsValue(property.Name))
                    {
                        ColumnsValue.Add(property.Name, property.GetValue(item));
                    }
                }
            }
        }

        /// <summary>
        /// 判断该实例是否在目标集合中被删除了 
        /// </summary>
        /// <returns></returns>
        public bool IsDeleted()
        {
            if (ColumnsValue.ContainsKey(this.Option.ColumnNameOfRemove))
            {
                return (bool)ColumnsValue[this.Option.ColumnNameOfRemove];
            }

            return false;
        }

        /// <summary>
        /// 依次比较两个实例中需要比较的字段的指
        /// </summary>
        /// <param name="item2"></param>
        /// <returns></returns>
        public bool IsDifferent(CompareItem<T1, T2> item2)
        {
            foreach (var item in ColumnsValue)
            {
                if (item.Key != this.Option.ColumnNameOfRemove)
                {
                    //如果对应item2的key2在item2的ColumnsValue中不存在，需要报错，可能打错列名了
                    string key2 = this.Option.ColumnsToCompare[item.Key];
                    if (!item2.ColumnsValue.ContainsKey(key2))
                    {
                        throw new Exception("Can not find the key to compare, please check your field:[ColumnsToCompare].");
                    }
                    else
                    {
                        var value1 = this.ColumnsValue[item.Key];
                        var value2 = item2.ColumnsValue[key2];

                        //TODO:此处应该实现一个根据类型比较
                        if (value1 is Enum)
                        {
                            int iValue1 = (int)value1;

                            if (!iValue1.Equals(value2))
                                return true;
                        }
                        else if (value1 == null)
                        {
                            if (value2 != null)
                            {
                                return true;
                            }
                            else
                                continue;
                        }
                        else if (!value1.Equals(value2))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class CompareSrcItem<T1, T2> : CompareItem<T1, T2>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="option"></param>
        public CompareSrcItem(object key, object item, CompareOption<T1, T2> option)
            : base(key, item, option, true)
        {
           
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class CompareDstItem<T1, T2> : CompareItem<T1, T2>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="option"></param>
        public CompareDstItem(object key, object item, CompareOption<T1, T2> option)
            : base(key, item, option, false)
        {

        }
    }

}
