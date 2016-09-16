using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kenpo.Extensions;
using Kenpo.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kenpo.Controllers
{
    public class CalendarController : Controller
    {
        [HttpGet, Route("api/calendar/urls")]
        public async Task<IActionResult> Urls()
        {
            var serviceCategory = new ServiceCategory();
            await serviceCategory.LoadAsync();
            await serviceCategory.ServiceGroup.LoadAsync();
            var urls = serviceCategory.ServiceGroup.ServiceApplies.Select(x => x.Url);
            return Json(urls);
        }

        [HttpGet, Route("api/calendar/events")]
        public async Task<IActionResult> Events(string url)
        {
            var serviceApply = new ServiceApply(new Uri(url));
            var serviceCategory = new ServiceCategory();
            await serviceApply.LoadAsync();
            foreach (var apply in serviceApply.Applies)
                await apply.LoadAsync();

            var source = new CalendarEventSource();
            source.Success = true;
            source.Result = serviceApply.Applies
                .SelectMany(x => x.Dates.Select(d => new { Apply = x, Date = d}))
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
                .ToArray();
            return Json(source);
        }
    }
}
