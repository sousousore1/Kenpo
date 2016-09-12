using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Kenpo.Models;
using Kenpo.Models.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kenpo.Controllers
{
    public class HomeController : Controller
    {
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
            foreach (var serviceApplyUrl in serviceApplyUrls)
            {
                var serviceApplyDom = await GetHtmlAsync(serviceApplyUrl);
                var applyUrls = serviceApplyDom.DocumentNode.SelectNodes("//li")
                    .SelectMany(x => x.Descendants("a"))
                    .Select(x => x.Attributes["href"].Value)
                    .Select(x => $"{rootUrl.Scheme}://{rootUrl.Host}" + x)
                    .Select(x => new Uri(x));
                    
                // apply
                foreach (var applyUrl in applyUrls)
                {
                    var applyDom = await GetHtmlAsync(applyUrl);
                    var title = applyDom.DocumentNode.SelectNodes("//table")
                        .Where(x => x.Attributes["class"] != null)
                        .Where(x => x.Attributes["class"].Value == "tform_new")
                        .Select(x => x.Descendants("tr").First())
                        .Select(x => x.Descendants("td").Last().InnerText)
                        .First();
                    var dates = applyDom.DocumentNode.SelectNodes("//select")
                        .Where(x => x.Attributes["id"] != null)
                        .Where(x => x.Attributes["id"].Value == "apply_join_time")
                        .SelectMany(x => x.Descendants("option"))
                        .Select(x => x.Attributes["value"].Value)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => Convert.ToDateTime(x));

                    var availability = new Availability();
                    availability.Title = title;
                    availability.Url = applyUrl;
                    availability.Dates = dates;
                    availabilities.Add(availability);
                }
            }

            var model = new IndexViewModel();
            model.Availabilities = availabilities;
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "このサイトはIT健保の保養所の空き日を検索するためのサイトです。";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private async Task<HtmlDocument> GetHtmlAsync(Uri url)
        {
            using (var client = new HttpClient())
            {
                var html = await client.GetStringAsync(url);
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);
                return htmlDoc;
            }
        }
    }
}
