using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity.EditorConfig
{
    public class EditorDateConfig : IEditorConfig
    {
        public DateTime Min { get; set; }
        public DateTime Max { get; set; }
    }
}
