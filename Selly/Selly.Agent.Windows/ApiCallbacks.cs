using System;
using Selly.Agent.API;
using Selly.Agent.API.DTO;
using Selly.Agent.Windows.Helpers;
using Selly.Agent.Windows.Models;
using System.Collections.Generic;
using System.Linq;

namespace Selly.Agent.Windows
{
    public class ApiCallbacks : IApiCallbacks
    {
        public EchoResponse Echo()
        {
            ToastHelper.PopToast("Echo");

            var response = new EchoResponse();
            response.Message = "Message";
            return response;
        }

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
                FirewallHelper.EnableFirewall();
            }
            else
            {
                FirewallHelper.DisableFirewall();
            }

            var response = new ConfigureResponse();
            response.FirewallEnabled = FirewallHelper.IsEnabled();
            return response;
        }        

        public GetRulesResponse GetRules()
        {
            ToastHelper.PopToast("Get rules");

            var rules = FirewallHelper.GetRules2();
            var response = new GetRulesResponse();
            response.WindowsResult = rules;
            return response;
        }

        public SetRuleResponse NewRule(SetRuleRequest dtoRule)
        {
            ToastHelper.PopToast("New rule");

            Rule domainRule;

            if (dtoRule.WindowsRequst != null)
            {
                domainRule = dtoRule.WindowsRequst;
            }
            else if (dtoRule.GenericRequest != null)
            {
                domainRule = (Rule)(new WindowsRuleAdapter(dtoRule.GenericRequest).NativeType);
            }
            else
            {
                return new SetRuleResponse() { Success = false };
            }

            FirewallHelper.NewRule(domainRule);
            return new SetRuleResponse() { Success = true };
        }

        public void UpdateRule(string name, SetRuleRequest dtoRule)
        {
            ToastHelper.PopToast("Update rule");

            Rule domainRule;

            if (dtoRule.WindowsRequst != null)
            {
                domainRule = dtoRule.WindowsRequst;
            }
            else if (dtoRule.GenericRequest != null)
            {
                domainRule = (Rule)(new WindowsRuleAdapter(dtoRule.GenericRequest).NativeType);
            }
            else
            {
                // TODO: This is inconsistent with NewRule
                return;
            }

            FirewallHelper.UpdateRule(name, domainRule);
        }

        public void DeleteRule(string id)
        {
            ToastHelper.PopToast("Delete rule");
            FirewallHelper.DeleteRule(id);
        }

        public SetRulesResponse UpdateRules(SetRulesRequest rules)
        {
            ToastHelper.PopToast("Update rules");

            var rulesToApply = rules.GenericRequest;
            if(rulesToApply.Count <= 0) { return new SetRulesResponse() { Success = false }; }

            List<Rule> domainRules = rulesToApply.Select( x => (Rule)(new WindowsRuleAdapter(x).NativeType) ).ToList();

            foreach (var rule in domainRules)
            {
                FirewallHelper.NewRule(rule);
            }

            return new SetRulesResponse() { Success = true };
        }
    }
}
