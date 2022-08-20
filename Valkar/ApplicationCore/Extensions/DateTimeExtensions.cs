using System;

namespace ApplicationCore.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDateInWeek(this DateTime value)
        {
            while (value.DayOfWeek != DayOfWeek.Monday)
            {
                value = value.AddDays(-1);
            }
            return value;
        }
    }
}
