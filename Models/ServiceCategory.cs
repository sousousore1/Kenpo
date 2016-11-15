using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Helpers;

namespace Kenpo.Models
{
    public class ServiceCategory
    {
        private static readonly Uri RootUrl = new Uri("https://as.its-kenpo.or.jp/service_category/index");

        public ServiceGroup ServiceGroup {get; private set;}

        public async Task<ServiceCategory> LoadAsync()
        {
            var serviceCategoryDom = await HtmlAgilityPackHelper.GetHtmlAsync(RootUrl);
            ServiceGroup = serviceCategoryDom.DocumentNode.SelectNodes("//a")
                .Where(x => x.InnerText == "直営・通年・夏季保養施設(空き照会)")
                .Select(x => x.Attributes["href"].Value)
                .Select(x => $"{RootUrl.Scheme}://{RootUrl.Host}" + x)
                .Select(x => new Uri(x))
                .Select(x => new ServiceGroup(x))
                .First();
            return this;
        }
    }
}
