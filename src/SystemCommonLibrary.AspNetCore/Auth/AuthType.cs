using System.ComponentModel;

namespace SystemCommonLibrary.AspNetCore.Auth
{
    public enum AuthType
    {
        [Description("内置")]
        Internal = 0,

        [Description("基本")]
        Basic = 1
    }
}
