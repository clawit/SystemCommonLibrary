using System.ComponentModel;

namespace SystemCommonLibrary.Enums
{
    public enum AuthType
    {
        [Description("内置")]
        Internal = 0,

        [Description("基本")]
        Basic = 1
    }
}
