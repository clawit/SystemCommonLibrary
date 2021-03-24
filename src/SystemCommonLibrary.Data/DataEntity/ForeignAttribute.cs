using System;

namespace SystemCommonLibrary.Data.DataEntity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignAttribute : Attribute
    {
        public string Foreign { get; set; }
        public string Key { get; set; }
        public string Display { get; set; }

        public ForeignAttribute(string foreign, string key, string display)
        {
            Foreign = foreign;
            Key = key;
            Display = display;
        }
    }
}
