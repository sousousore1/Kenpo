using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kenpo.Models
{
    public class CalendarEventSource {
        public bool Success {get; set;}
        public IEnumerable<CalendarEvent> Result {get; set;}
    }
}
