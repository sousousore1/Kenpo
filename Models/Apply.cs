using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Helpers;

namespace Kenpo.Models
{
    public class Apply
    {
        public Apply(Uri url)
        {
            Url = url;
        }

        public Uri Url {get;}
        public string Title {get; private set;}
        public IEnumerable<DateTime> Dates {get; private set;}

        public async Task<Apply> LoadAsync()
        {
            // apply
            var applyDom = await HtmlAgilityPackHelper.GetHtmlAsync(Url);
            Title = applyDom.DocumentNode.SelectNodes("//table")
                .Where(x => x.Attributes["class"] != null)
                .Where(x => x.Attributes["class"].Value == "tform_new")
                .Select(x => x.Descendants("tr").First())
                .Select(x => x.Descendants("td").Last().InnerText)
                .First();
            Dates = applyDom.DocumentNode.SelectNodes("//select")
                .Where(x => x.Attributes["id"] != null)
                .Where(x => x.Attributes["id"].Value == "apply_join_time")
                .SelectMany(x => x.Descendants("option"))
                .Select(x => x.Attributes["value"].Value)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Convert.ToDateTime(x))
                .ToArray();
            return this;
        }
    }
}
