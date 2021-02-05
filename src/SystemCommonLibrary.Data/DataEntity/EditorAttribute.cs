using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorAttribute : Attribute
    {
        public bool Editable { get; set; } = true;
        public EditorType EditorType { get; }
        public bool Required { get; set; } = false;
        public int Length { get; set; } = 0;

        /// <summary>
        /// 可能的元素列表
        /// </summary>
        public Dictionary<string, object> Items { get; } = null;

        /// <summary>
        /// 可能的最小值
        /// </summary>
        public object Min { get; set; } = null;
        /// <summary>
        /// 可能的最大值
        /// </summary>
        public object Max { get; set; } = null;

        /// <summary>
        /// 精确到小数位数
        /// </summary>
        public int Scale { get; set; } = 0;

        public EditorAttribute(EditorType type, string items=null)
        {
            this.EditorType = type;

            if (!string.IsNullOrEmpty(items))
            {
                items = items.Trim();
                if (items.StartsWith('{') && items.EndsWith('}'))
                {
                    this.Items = new Dictionary<string, object>();
                    
                    var pairs = items.Substring(1, items.Length - 2)
                            .Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var pair in pairs)
                    {
                        var kv = pair.Trim();
                        if (kv.StartsWith('{') && kv.EndsWith('}'))
                        {
                            var kvs = kv.Substring(1, kv.Length - 2)
                                        .Split(':', StringSplitOptions.RemoveEmptyEntries);
                            if (kvs.Length == 2)
                            {
                                string key = kvs[0].Trim();
                                string value = kvs[1].Trim();
                                this.Items.Add(key, value);
                            }
                            else
                                throw new ArgumentException();
                        }
                        else
                            throw new ArgumentException();
                    }
                    if (this.Items.Count > 0)
                    {
                        //尝试对全是数值的values进行转换
                        if (this.Items.Values.All(v => decimal.TryParse(v.ToString(), out _)))
                        {
                            for (int i = 0; i < this.Items.Count; i++)
                            {
                                var key = this.Items.Keys.ElementAt(i);
                                decimal.TryParse(this.Items[key].ToString(), out var value);
                                this.Items[key] = value;
                            }
                        }
                    }
                    else
                        this.Items = null;
                }
                else
                    throw new ArgumentException();

            }
        }
    }
}
