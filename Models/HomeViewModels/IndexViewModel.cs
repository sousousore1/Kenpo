using System;

namespace Kenpo.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public string Header => new AvailabilityDateTime(DateTime.Now).Value.ToString("yyyy年MM月dd日 HH:mm時点の空き情報");
    }
}
