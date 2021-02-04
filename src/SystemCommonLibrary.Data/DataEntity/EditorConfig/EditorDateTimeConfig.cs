using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity.EditorConfig
{
    public class EditorDateTimeConfig : IEditorConfig
    {
        public DateTime Min { get; set; }
        public DateTime Max { get; set; }
    }
}
