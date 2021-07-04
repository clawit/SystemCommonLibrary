using System;

namespace SystemCommonLibrary.Enums
{
    public enum EditorType
    {
        None = 0, 
        Text = 1,           //文本
        Number = 2,         //数字
        Switch = 3,         //开关 Bool
        Date = 4,           //日期
        Time = 5,           //时间
        DateTime = 6,       //日期时间
        List = 7,           //单选 枚举
        Checkbox = 8,       //多选 枚举(Flags)
        Image = 9,          //图片
        Flyer = 10,         //单页
        Json = 11,          //json
        Icon = 12,          //图标
        EntityFilter = 13,   //实体筛选

    }

}
