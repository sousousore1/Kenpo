using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Configurations;
using Microsoft.Extensions.Options;

namespace Kenpo.Models
{
    public abstract class ViewModelBase
    {
        private readonly IOptions<AppSettings> _config;

        protected ViewModelBase(IOptions<AppSettings> config)
        {
            _config = config;
        }

        public string Title => _config.Value.ApplicationName;
    }
}
