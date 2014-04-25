using System;

namespace Tax.WebAPI.Helpers
{
    public class TimestampHelpers
    {
        public static string GetTimestamp(DateTime DateTimeFormat)
        {
            return ((DateTimeFormat.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000000).ToString();
        }
    }
}