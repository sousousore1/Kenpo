using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Helpers;

namespace Kenpo.Models
{
    public class ServiceApply
    {
        public ServiceApply(Uri url)
        {
            Url = url;
        }

        public Uri Url {get;}

        public IEnumerable<Apply> Applies {get; private set;}

        public async Task<ServiceApply> LoadAsync()
        {
            // service_apply
            var serviceApplyDom = await HtmlAgilityPackHelper.GetHtmlAsync(Url);
            Applies = serviceApplyDom.DocumentNode.SelectNodes("//li")
                .SelectMany(x => x.Descendants("a"))
                .Select(x => x.Attributes["href"].Value)
                .Select(x => $"{Url.Scheme}://{Url.Host}" + x)
                .Select(x => new Uri(x))
                .Select(x => new Apply(x))
                .ToArray();
            return this;
        }
    }
}
