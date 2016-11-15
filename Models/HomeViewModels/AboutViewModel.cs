using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Configurations;
using Microsoft.Extensions.Options;

namespace Kenpo.Models.HomeViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(IOptions<AppSettings> config) : base(config)
        {
        }
    }
}
