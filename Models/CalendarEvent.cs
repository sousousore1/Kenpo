using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kenpo.Models
{
    public class CalendarEvent
    {
        public string Id {get; set;}
        public string Title {get; set;}
        public string Url {get; set;}
        public string Class {get; set;}
        public double Start {get; set;}
        public double End {get; set;}
    }
}
