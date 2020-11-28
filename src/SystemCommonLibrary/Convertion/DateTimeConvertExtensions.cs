using System;

namespace SystemCommonLibrary.Convertion
{
    public static class DateTimeConvertExtensions
    {
        public static double ToUnixEpoch(this DateTime dateTime)
        {
            return (dateTime - DateTime.UnixEpoch.ToLocalTime()).TotalSeconds;
        }

        public static DateTime ToLocalTime(this double unitTimestamp)
        {
            return DateTime.UnixEpoch.ToLocalTime().AddSeconds(unitTimestamp);
        }

    }
}
