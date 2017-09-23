using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selly.Agent.API.DTO;
using Selly.Agent.Generic.Models;
using Selly.Agent.Linux.Models;
using Selly.Agent.Windows.Models;
using Selly.NMS.Web.Infrastructure;
using Selly.NMS.Web.Models;
using Selly.NMS.Web.ViewModels.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Controllers
{
    [Authorize]
    public class RulesController : Controller
    {
        IDeviceRepository repo;
        IEventsRepository eventsRepo;

        public RulesController(IDeviceRepository repository, IEventsRepository eventsRepository)
        {
            repo = repository;
            eventsRepo = eventsRepository;
        }

        public async Task<IActionResult> Index(string id)
        {
            var device = repo.GetDevice(id);
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
                return View(new IndexVM()
                {
                    Device = device,
                    Rules = rules
                });
            }
            catch(Exception e)
            {
                return RedirectToAction("View", "Devices", new { Id = id });
            }

        }

        public IActionResult Create(string id)
        {
            var device = repo.Devices.FirstOrDefault(dev => dev.Id == id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string id, Agent.Generic.Models.Rule rule)
        {
            var device = repo.GetDevice(id);

            try
            {
                rule.LocalPort = rule.LocalPort ?? Agent.Generic.Models.Rule.ANY_PORT;
                rule.RemoteAddress = System.Net.IPAddress.Parse(rule.RemoteAddress).ToString();
            }
            catch (FormatException)
            {
                rule.RemoteAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }

            // TODO: Add to UI
            if (rule.Direction == Agent.Generic.Models.Direction.In)
            {
                rule.RemotePort = Agent.Generic.Models.Rule.ANY_PORT;
                rule.LocalAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }
            else
            {
                rule.RemotePort = rule.LocalPort;

                rule.LocalPort = Agent.Generic.Models.Rule.ANY_PORT;
                rule.LocalAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }
            
            var dtoRule = new SetRuleRequest();
            dtoRule.GenericRequest = rule;

            var client = new AgentApiClient();
            await client.NewRule(device.Address, dtoRule);

            return RedirectToAction("Index");
        }

        public IActionResult View(string id, string name)
        {
            // TODO: I thought MVC did this automatically...
            // Remove any extra percent coding
            var unescapedRuleName = Uri.UnescapeDataString(name);


            var device = repo.GetDevice(id);
            if (device == null) { throw new ArgumentOutOfRangeException(); }

            var rules = HttpContext.Session.GetJson<List<Agent.Generic.Models.Rule>>("rules");
            var selectedRule = rules.FirstOrDefault(r => r.Name == unescapedRuleName);

            // TODO: Add to UI
            if(selectedRule.Direction == Agent.Generic.Models.Direction.Out)
            {
                ushort? tmp = selectedRule.LocalPort;
                selectedRule.LocalPort = selectedRule.RemotePort;
                selectedRule.RemotePort = tmp;
            }

            ICollection<PacketDroppedEvent> events = null;

            if(selectedRule.FilterID != null)
            {
                events = eventsRepo.GetEvents(device.Id, selectedRule.FilterID);
                ViewData["MostTriggeredBy"] = eventsRepo.RuleMostTriggeredBy(device.Id, selectedRule.FilterID) ?? "Untriggered";

                var data = eventsRepo.GetEventsForTheLast24Hours(device.Id, selectedRule.FilterID);
                var last24Hours = GoogleChartHelpers.To24HourArray(data);
                ViewData["Last24Hours"] = GoogleChartHelpers.ToGoogleChartString(last24Hours, "Hour", "Count");
            }
            else
            {
                events = eventsRepo.GetEvents(device.Id, selectedRule.RemoteAddress, Convert.ToInt32(selectedRule.LocalPort));
                ViewData["MostTriggeredBy"] = eventsRepo.RuleMostTriggeredBy(device.Id, selectedRule.RemoteAddress, Convert.ToInt32(selectedRule.LocalPort)) 
                                                ?? "Untriggered";

                var data = eventsRepo.GetEventsForTheLast24Hours(device.Id, selectedRule.RemoteAddress, Convert.ToInt32(selectedRule.LocalPort));
                var last24Hours = GoogleChartHelpers.To24HourArray(data);
                ViewData["Last24Hours"] = GoogleChartHelpers.ToGoogleChartString(last24Hours, "Hour", "Count");
            }

            ViewData["TimesFired"] = events.Count;
            ViewData["LastFired"] = events.FirstOrDefault()?.Time.ToString() ?? "Never";
            
            return View(selectedRule);
        }

        [HttpPost]
        public async Task<IActionResult> View([FromRoute] string id, [FromRoute] string name, Agent.Generic.Models.Rule rule)
        {
            var device = repo.GetDevice(id);

            try
            {
                rule.LocalPort = rule.LocalPort ?? Agent.Generic.Models.Rule.ANY_PORT;
                rule.RemoteAddress = System.Net.IPAddress.Parse(rule.RemoteAddress).ToString();
            }
            catch (FormatException)
            {
                rule.RemoteAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }

            // TODO: Add to UI
            if (rule.Direction == Agent.Generic.Models.Direction.In)
            {
                rule.RemotePort = Agent.Generic.Models.Rule.ANY_PORT;
                rule.LocalAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }
            else
            {
                rule.RemotePort = rule.LocalPort;

                rule.LocalPort = Agent.Generic.Models.Rule.ANY_PORT;
                rule.LocalAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }

            var client = new AgentApiClient();
            await client.UpdateRule(device.Address, name, new SetRuleRequest()
            {
                GenericRequest = rule
            });

            return RedirectToAction("Index");
        }

        public IActionResult ConfirmDeletion(string id, string name)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string name)
        {
            var device = repo.Devices.FirstOrDefault(dev => dev.Id == id);
            var client = new AgentApiClient();
            await client.DeleteRule(device.Address, name);

            return RedirectToAction("Index");
        }
    }
}
