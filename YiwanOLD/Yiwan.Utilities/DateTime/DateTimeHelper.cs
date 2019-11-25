using System;

namespace Yiwan.Utilities
{
    public class DateTimeHelper
    {
        public static long GetTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime().Ticks) / 10000;
        }
        public static long GetTimestamp(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(dateTime - startTime).TotalSeconds;
        }
        public static long GetTimestampMs(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(dateTime - startTime).TotalMilliseconds;
        }
        public static DateTime GetTime(long timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return startTime.AddSeconds(timeStamp);
        }
        public static DateTime GetTimeMs(long timeStamp)
        {
            DateTime startTime = new DateTime(1970, 1, 1);
            return startTime.AddMilliseconds(timeStamp);
        }
    }
}
