using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Helpers;

namespace Kenpo.Models
{
    public class ServiceGroup
    {
        public ServiceGroup(Uri url)
        {
            Url = url;
        }

        public Uri Url {get;}

        public IEnumerable<ServiceApply> ServiceApplies {get; private set;}

        public async Task<ServiceGroup> LoadAsync()
        {
            // service_group
            var serviceGroupDom = await HtmlAgilityPackHelper.GetHtmlAsync(Url);
            ServiceApplies = serviceGroupDom.DocumentNode.SelectNodes("//li")
                .SelectMany(x => x.Descendants("a"))
                .Select(x => x.Attributes["href"].Value)
                .Select(x => $"{Url.Scheme}://{Url.Host}" + x)
                .Select(x => new Uri(x))
                .Select(x => new ServiceApply(x))
                .ToArray();
            return this;
        }
    }
}
