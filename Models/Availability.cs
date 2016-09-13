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
        public IEnumerable<AvailabilityDateTime> Dates {get;set;}
    }
}
