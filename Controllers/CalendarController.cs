using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Extensions;
using Kenpo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kenpo.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public CalendarController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet, Route("api/calendar/urls")]
        public async Task<IActionResult> Urls()
        {
            var key = "urls";
            IEnumerable<Uri> urls;
            if (_memoryCache.TryGetValue(key, out urls))
                return Json(urls);

            var serviceCategory = new ServiceCategory();
            await serviceCategory.LoadAsync();
            await serviceCategory.ServiceGroup.LoadAsync();
            urls = serviceCategory.ServiceGroup.ServiceApplies.Select(x => x.Url);
            _memoryCache.Set(key, urls, TimeSpan.FromSeconds(10));
            return Json(urls);
        }

        [HttpGet, Route("api/calendar/events")]
        public async Task<IActionResult> Events(string url)
        {
            var key = $"source-{url}";
            CalendarEventSource source;
            if (_memoryCache.TryGetValue(key, out source))
                return Json(source);

            var serviceApply = new ServiceApply(new Uri(url));
            await serviceApply.LoadAsync();
            foreach (var apply in serviceApply.Applies)
                await apply.LoadAsync();

            source = new CalendarEventSource
            {
                Success = true,
                Result = serviceApply.Applies
                    .SelectMany(x => x.Dates.Select(d => new {Apply = x, Date = d}))
                    .Select(x =>
                        new CalendarEvent
                        {
                            Id = x.Apply.Title,
                            Title = x.Apply.Title,
                            Url = x.Apply.Url.ToString(),
                            Class = x.Date.ToClassName(),
                            Start = x.Date.ToTotalMilliseconds(),
                            End = x.Date.ToTotalMilliseconds(),
                        })
                    .ToArray()
            };
            _memoryCache.Set(key, source, TimeSpan.FromSeconds(10));
            return Json(source);
        }
    }
}
