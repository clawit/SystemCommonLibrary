using System;
using System.Collections.Generic;

namespace SystemCommonLibrary.Data.DataEntity
{
    public class EntityColumn
    {
        public EntityColumn()
        {
            
        }
        public EntityColumn(string column)
        {
            Column = column;
        }

        public string Column { get; set; }
        public string Name { get; set; }
        public bool Hidden { get; set; }
        public string Formatter { get; set; }
        public bool Editable { get; set; }
        public EditorType Editor { get; set; }
        public bool IsKey { get; set; } = false;
        public int Length { get; set; } = 0;
        public bool Required { get; set; } = false;

        /// <summary>
        /// 可能的元素列表
        /// </summary>
        public Dictionary<string, object> Items { get; set; } = null;
        
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
        /// <summary>
        /// 单次调整值的步进
        /// </summary>
        public decimal Step { get; set; } = 0;

    }
}
