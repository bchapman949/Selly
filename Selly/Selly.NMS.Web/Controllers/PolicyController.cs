using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Selly.Agent.API.DTO;
using Selly.NMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Controllers
{
    public class PolicyController : Controller
    {
        IDeviceRepository deviceRepo;
        IEventsRepository eventRepo;
        IPolicyRepository policyRepo;

        public PolicyController(IDeviceRepository deviceRepository, IEventsRepository eventRepository, IPolicyRepository policyRepository)
        {
            deviceRepo = deviceRepository;
            eventRepo = eventRepository;
            policyRepo = policyRepository;
        }

        public IActionResult Index()
        {
            var policies = policyRepo.Policies;
            return View(policies);
        }

        public IActionResult CrossDeviceRule()
        {
            var devices = deviceRepo.Devices.ToList();
            ViewBag.Devices = devices;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrossDeviceRule(Agent.Generic.Models.Rule rule)
        {
            var f = Request.Form;
            var devs = f["Device"];

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

            var tasks = new List<Task>(devs.Count);

            foreach (var device in devs)
            {
                var d = deviceRepo.GetDevice(device);
                tasks.Add(client.NewRule(d.Address, dtoRule));
            }

            await Task.WhenAll(tasks);

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Policy policy)
        {
            policyRepo.AddPolicy(policy);
            return RedirectToAction("Index");
        }

        public IActionResult ConfirmDeletion(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            policyRepo.DeletePolicy(id);
            return RedirectToAction("Index");
        }

        public IActionResult ApplyPolicy(string id)
        {
            var devices = deviceRepo.Devices.ToList();
            ViewBag.Devices = devices;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyTo(string id)
        {
            var policy = policyRepo.GetPolicy(id);
            
            if(policy.Rules.Count <= 0) { return RedirectToAction("Index"); }

            var f = Request.Form;
            var devs = f["Device"];

            var rulesRequest = new SetRulesRequest();
            var client = new AgentApiClient();
            var tasks = new List<Task>(devs.Count);

            rulesRequest.GenericRequest = Mapper.Map<List<Selly.Agent.Generic.Models.Rule>>(policy.Rules.ToList());

            foreach (var device in devs)
            {
                var d = deviceRepo.GetDevice(device);
                tasks.Add(client.SetRulesRequest(d.Address, rulesRequest));
            }

            await Task.WhenAll(tasks);
            return RedirectToAction("Index");
        }
    }
}
