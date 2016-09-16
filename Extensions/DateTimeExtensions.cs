using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kenpo.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToTotalMilliseconds(this DateTime date)
        {
            return date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public static string ToClassName(this DateTime date)
        {
            switch(date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "event-important";
                case DayOfWeek.Sunday:
                    return "event-success";
                default:
                    return string.Empty;
            }
        }
    }
}
