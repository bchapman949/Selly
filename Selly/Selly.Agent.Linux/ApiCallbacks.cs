using Selly.Agent.API;
using Selly.Agent.API.DTO;
using Selly.Agent.Linux.Helpers;
using Selly.Agent.Linux.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selly.Agent.Linux
{
    public class ApiCallbacks : IApiCallbacks
    {
        public GetConfigurationResponse GetConfiguration()
        {
            ToastHelper.PopToast("Get configuration");

            var response = new GetConfigurationResponse();
            response.FirewallEnabled = FirewallHelper.IsEnabled();

            return response;
        }

        public ConfigureResponse Configure(ConfigureRequest request)
        {
            ToastHelper.PopToast("Configure");

            if(request.FirewallEnabled)
            {
                FirewallHelper.Enable();
            }
            else
            {
                FirewallHelper.Disable();
            }

            return new ConfigureResponse()
            {
                FirewallEnabled = FirewallHelper.IsEnabled()
            };
        }

        public void DeleteRule(string id)
        {
            ToastHelper.PopToast("Delete rule");
            FirewallHelper.DeleteRule(Convert.ToUInt32(id));
        }

        public EchoResponse Echo()
        {
            ToastHelper.PopToast("Echo");
            return new EchoResponse()
            {
                Message = "Message"
            };
        }

        public GetRulesResponse GetRules()
        {
            ToastHelper.PopToast("Get rules");
            //var chain = FirewallHelper.GetRules();
            //var dtoRules = chain.ToModel();

            var rules = FirewallHelper.GetRules();


            var response = new GetRulesResponse()
            {
                UfwResult = rules
            };

            return response;
        }

        public SetRuleResponse NewRule(SetRuleRequest rule)
        {
            ToastHelper.PopToast("New rule");

            Linux.Models.Rule newEntry = null;

            if (rule.GenericRequest != null)
            {
                newEntry = (Linux.Models.Rule)(new LinuxRuleAdapter(rule.GenericRequest).NativeType);
            }
            else if (rule.UfwRequest != null)
            {
                newEntry = rule.UfwRequest;
            }

            //var st = Mapper.Map<FirewallAPI.Rule>(newEntry);
            FirewallHelper.NewRule(newEntry);
            return new SetRuleResponse()
            {
                Success = true
            };
        }

        public void UpdateRule(string id, SetRuleRequest rule)
        {
            Linux.Models.Rule newEntry = null;

            if (rule.GenericRequest != null)
            {
                newEntry = (Linux.Models.Rule)(new LinuxRuleAdapter(rule.GenericRequest).NativeType);
            }
            else if (rule.UfwRequest != null)
            {
                newEntry = rule.UfwRequest;
            }

            FirewallHelper.UpdateRule(newEntry);
        }

        public SetRulesResponse UpdateRules(SetRulesRequest rules)
        {
             ToastHelper.PopToast("Update rules");

            var rulesToApply = rules.GenericRequest;
            if(rulesToApply.Count <= 0) { return new SetRulesResponse() { Success = false }; }

            List<Rule> domainRules = rulesToApply.Select( x => (Rule)(new LinuxRuleAdapter(x).NativeType) ).ToList();

            foreach (var rule in domainRules)
            {
                FirewallHelper.NewRule(rule);
            }

            return new SetRulesResponse() { Success = true };
        }
    }
}
