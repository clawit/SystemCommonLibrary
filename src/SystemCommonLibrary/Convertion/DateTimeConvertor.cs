using System;

namespace SystemCommonLibrary.Convertion
{
    public static class DateTimeConvertor
    {
        public static double ToUnixEpoch(this DateTime dateTime)
        {
            return (dateTime - DateTime.UnixEpoch).TotalSeconds;
        }

        public static DateTime FromUnixEpoch(this double unitTimestamp)
        {
            return DateTime.UnixEpoch.AddSeconds(unitTimestamp);
        }

    }
}
