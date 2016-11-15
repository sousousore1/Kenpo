using System;
using Kenpo.Configurations;
using Microsoft.Extensions.Options;

namespace Kenpo.Models.HomeViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel(IOptions<AppSettings> config) : base(config)
        {
        }

        public string Header => DateTime.Now.ToString("yyyy年MM月dd日 HH:mm時点の空き情報");
    }
}
