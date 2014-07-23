using System;

namespace Tax.WebAPI.Helpers
{
    public class TimestampHelpers
    {
        public static string GetTimestamp(DateTime DateTimeFormat)
        {
            DateTime utc = TimeZoneInfo.ConvertTimeToUtc(DateTimeFormat, TimeZoneInfo.Local);
            return ((utc.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000).ToString();
        }
    }
}