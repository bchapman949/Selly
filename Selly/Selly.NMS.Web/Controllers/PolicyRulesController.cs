using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Selly.NMS.Web.Models;
using Selly.NMS.Web.ViewModels.PolicyRule;
using System;
using System.Collections.Generic;

namespace Selly.NMS.Web.Controllers
{
    public class PolicyRulesController : Controller
    {
        IDeviceRepository deviceRepo;
        IEventsRepository eventRepo;
        IPolicyRepository policyRepo;

        public PolicyRulesController(IDeviceRepository deviceRepository, IEventsRepository eventRepository, IPolicyRepository policyRepository)
        {
            deviceRepo = deviceRepository;
            eventRepo = eventRepository;
            policyRepo = policyRepository;
        }

        public IActionResult Index(string id)
        {
            var policy = policyRepo.GetPolicy(id);

            ViewBag.Rules = Mapper.Map<List<ViewModels.PolicyRule.PolicyRuleVM>>(policy.Rules);

            return View(policy);
        }

        public IActionResult CreatePolicyRule()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePolicyRule(string id, Agent.Generic.Models.Rule rule)
        {
            try
            {
                rule.LocalAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
                rule.LocalPort = rule.LocalPort ?? Agent.Generic.Models.Rule.ANY_PORT;
                rule.RemoteAddress = System.Net.IPAddress.Parse(rule.RemoteAddress).ToString();
            }
            catch (FormatException)
            {
                rule.RemoteAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }

            // TODO: Add to UI - Correct Address
            if (rule.Direction == Agent.Generic.Models.Direction.In)
            {
                rule.RemotePort = Agent.Generic.Models.Rule.ANY_PORT;
            }
            else
            {
                rule.RemotePort = rule.LocalPort;
                rule.LocalPort = Agent.Generic.Models.Rule.ANY_PORT;
            }

            var policyRule = Mapper.Map<PolicyRule>(rule);
            policyRule.PolicyId = id;

            policyRepo.AddPolicyRule(policyRule);
            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult View(string id, string ruleId)
        {
            var policyRule = policyRepo.GetPolicyRule(id, ruleId);

            var vm = Mapper.Map<PolicyRuleVM>(policyRule);

            return View(vm);
        }

        [HttpPost]
        public IActionResult View(string id, string ruleId, PolicyRuleVM rule)
        {
            try
            {
                rule.LocalAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
                rule.LocalPort = rule.LocalPort ?? Agent.Generic.Models.Rule.ANY_PORT;
                rule.RemoteAddress = System.Net.IPAddress.Parse(rule.RemoteAddress).ToString();
            }
            catch (FormatException)
            {
                rule.RemoteAddress = Agent.Generic.Models.Rule.ANY_IP_ADDRESS;
            }

            // TODO: Add to UI - Correct Address
            if (rule.Direction == Agent.Generic.Models.Direction.In)
            {
                rule.RemotePort = Agent.Generic.Models.Rule.ANY_PORT;
            }
            else
            {
                rule.RemotePort = rule.LocalPort;
                rule.LocalPort = Agent.Generic.Models.Rule.ANY_PORT;
            }

            var policyRule = Mapper.Map<PolicyRule>(rule);

            // TODO: Poor database modelling and routing issue consequence
            policyRule.Id = ruleId;
            policyRule.PolicyId = id;
            
            policyRepo.UpdatePolicyRule(policyRule);
            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult ConfirmDeletion(string id, string ruleId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string id, string ruleId)
        {
            policyRepo.DeletePolicyRule(ruleId);
            return RedirectToAction("Index", new { id = id });
        }
    }
}
