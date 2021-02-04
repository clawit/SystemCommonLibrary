using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.DataEntity.EditorConfig
{
    public class EditorListConfig : IEditorConfig
    {
        public List<EditorConfigItem> Items { get; set; } = new List<EditorConfigItem>();
    }
}
