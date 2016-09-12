using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kenpo.Models
{
    public class Availability
    {
        public string Title {get; set;}
        public Uri Url {get; set;}
        public IEnumerable<DateTime> Dates {get;set;}

        public string ToString(DateTime date)
        {
            return date.ToString("yyyy年MM月dd日(dddd)");
        }

        public string ToClass(DateTime date)
        {
            switch(date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "text-info";
                case DayOfWeek.Sunday:
                    return "text-danger";
                default:
                    return string.Empty;
            }
        }
    }
}
