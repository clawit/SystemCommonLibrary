using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity.EditorConfig
{
    public class EditorNumberConfig : IEditorConfig
    {
        public decimal Min { get; set; } = decimal.MinValue;

        public decimal Max { get; set; } = decimal.MaxValue;

        /// <summary>
        /// 精确到小数位数
        /// </summary>
        public int Scale { get; set; } = 0;

        /// <summary>
        /// 单次调整值的步进
        /// </summary>
        public int Step { get; set; } = 0;

    }
}
