using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kenpo.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public string Header => DateTime.Now.ToString("yyyy年MM月dd日 HH:mm時点の空き情報");
        public IEnumerable<Availability> Availabilities {get; set;}
    }
}
