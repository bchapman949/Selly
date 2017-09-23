using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selly.Agent.Generic.Models;
using Selly.Agent.Linux.Models;
using Selly.Agent.Windows.Models;
using Selly.NMS.Web.Infrastructure;
using Selly.NMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        IDeviceRepository deviceRepo;
        IEventsRepository eventRepo;

        public EventsController(IDeviceRepository deviceRepository, IEventsRepository eventRepository)
        {
            deviceRepo = deviceRepository;
            eventRepo = eventRepository;
        }

        public IActionResult Index(string id)
        {
            var device = deviceRepo.GetDevice(id);
            if (device == null) { throw new ArgumentException(); }

            var events = eventRepo.GetEvents(id);

            return View(new ViewModels.Events.IndexVM()
            {
                Device = device,
                Events = events
            });
        }

        public new async Task<IActionResult> View(string id)
        {
            var ev = eventRepo.GetEvent(id);
            if (ev == null) { throw new ArgumentException(); }

            var device = deviceRepo.GetDevice(ev.DeviceId);
            if (device == null) { throw new ArgumentException(); }

            if(! string.IsNullOrEmpty(ev.FilterName))
            {
                bool isInRules = await TryFindInRules(device, ev);

                if (isInRules)
                {
                    return RedirectToAction("View", "Rules", new { id = device.Id, name = ev.FilterName });
                }
            }
            

            var data = eventRepo.GetEventsForTheLast24Hours(device.Id, ev.FilterName);
            var last24Hours = GoogleChartHelpers.To24HourArray(data);
            ViewData["Last24Hours"] = GoogleChartHelpers.ToGoogleChartString(last24Hours, "Time", "Count");

            return View(ev);
        }

        private async Task<bool> TryFindInRules(Device device, PacketDroppedEvent ev)
        {
            var client = new AgentApiClient();
            try
            {
                var dtoRules = await client.GetRules(device.Address);

                List<IRule> rules;

                if (dtoRules.UfwResult != null)
                {
                    rules = dtoRules.UfwResult.Select(rule => (IRule)new LinuxRuleAdapter(rule)).ToList();
                }
                else
                {
                    rules = dtoRules.WindowsResult.Select(entry => (IRule)new WindowsRuleAdapter(entry)).ToList();
                }

                rules = rules.OrderBy(x => x.Direction).ToList();
                HttpContext.Session.SetJson("rules", rules);

                var matchingRule = rules.FirstOrDefault(x => x.Name == ev.FilterName);
                if(matchingRule != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
