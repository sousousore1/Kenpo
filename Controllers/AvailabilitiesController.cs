using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Kenpo.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kenpo.Controllers
{
    public class AvailabilitiesController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // service_category
            var rootUrl = new Uri("https://as.its-kenpo.or.jp/service_category/index");
            var serviceCategoryDom = await GetHtmlAsync(rootUrl);
            var serviceGroupUrl = serviceCategoryDom.DocumentNode.SelectNodes("//a")
                .Where(x => x.InnerText == "直営・通年・夏季保養施設(空き照会)")
                .Select(x => x.Attributes["href"].Value)
                .Select(x => $"{rootUrl.Scheme}://{rootUrl.Host}" + x)
                .Select(x => new Uri(x))
                .First();

            // service_group
            var serviceGroupDom = await GetHtmlAsync(serviceGroupUrl);
            var serviceApplyUrls = serviceGroupDom.DocumentNode.SelectNodes("//li")
                .SelectMany(x => x.Descendants("a"))
                .Select(x => x.Attributes["href"].Value)
                .Select(x => $"{rootUrl.Scheme}://{rootUrl.Host}" + x)
                .Select(x => new Uri(x));
            
            // service_apply
            var availabilities = new List<Availability>();
            var serviceApplyDoms = await Task.WhenAll(serviceApplyUrls.Select(x => GetHtmlAsync(x)));
            foreach (var serviceApplyDom in serviceApplyDoms)
            {
                var applyUrls = serviceApplyDom.DocumentNode.SelectNodes("//li")
                    .SelectMany(x => x.Descendants("a"))
                    .Select(x => x.Attributes["href"].Value)
                    .Select(x => $"{rootUrl.Scheme}://{rootUrl.Host}" + x)
                    .Select(x => new Uri(x));
                    
                // apply
                var applyDoms = await Task.WhenAll(applyUrls.Select(async x => new { Url = x, Dom = await GetHtmlAsync(x)}));
                foreach (var applyDom in applyDoms)
                {
                    var title = applyDom.Dom.DocumentNode.SelectNodes("//table")
                        .Where(x => x.Attributes["class"] != null)
                        .Where(x => x.Attributes["class"].Value == "tform_new")
                        .Select(x => x.Descendants("tr").First())
                        .Select(x => x.Descendants("td").Last().InnerText)
                        .First();
                    var dates = applyDom.Dom.DocumentNode.SelectNodes("//select")
                        .Where(x => x.Attributes["id"] != null)
                        .Where(x => x.Attributes["id"].Value == "apply_join_time")
                        .SelectMany(x => x.Descendants("option"))
                        .Select(x => x.Attributes["value"].Value)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => new AvailabilityDateTime(Convert.ToDateTime(x)));

                    var availability = new Availability();
                    availability.Title = title;
                    availability.Url = applyDom.Url;
                    availability.Dates = dates;
                    availabilities.Add(availability);
                }
            }
            
            return Json(availabilities.OrderBy(x => x.Title));
        }

        private async Task<HtmlDocument> GetHtmlAsync(Uri url)
        {
            using (var client = new HttpClient())
            {
                using (var stream = await client.GetStreamAsync(url))
                {
                    var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.Load(stream);
                    return htmlDoc;
                }
            }
        }
    }
}
