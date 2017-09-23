using AutoMapper;
using System;
using System.Collections.Generic;

namespace Selly.Agent.Linux.Helpers
{
    public class FirewallHelper
    {
        public static bool IsEnabled()
        {
            return FirewallAPI.FirewallManager.IsEnabled();
        }

        public static void Enable()
        {
            FirewallAPI.FirewallManager.Enable();
        }

        public static void Disable()
        {
            FirewallAPI.FirewallManager.Disable();
        }

        public static List<Models.Rule> GetRules()
        {
            var firewallRules = FirewallAPI.FirewallManager.GetRules();
            var domainRules = Mapper.Map<List<Models.Rule>>(firewallRules);

            foreach (var rule in domainRules)
            {
                if (rule.LocalAddress == FirewallAPI.Rule.ANY_IP_ADDRESS)
                {
                    rule.LocalAddress = Models.Rule.ANY_IP_ADDRESS;
                }

                // TODO: See if we can just change the type in the domain model
                if (rule.LocalPort == FirewallAPI.Rule.ANY_PORT)
                {
                    rule.LocalPort = Models.Rule.ANY_PORT;
                }

                if (rule.RemoteAddress == FirewallAPI.Rule.ANY_IP_ADDRESS)
                {
                    rule.RemoteAddress = Models.Rule.ANY_IP_ADDRESS;
                }

                if (rule.RemotePort == FirewallAPI.Rule.ANY_PORT)
                {
                    rule.RemotePort = Models.Rule.ANY_PORT;
                }
            }

            return domainRules;
        }

        public static void NewRule(Models.Rule domainRule)
        {
            Console.WriteLine($"DOMAIN RULE: {domainRule}");

            var firewallRule = Mapper.Map<FirewallAPI.Rule>(domainRule);

            Console.WriteLine($"Pre-FW RULE: {firewallRule}");

            if(domainRule.LocalAddress == Models.Rule.ANY_IP_ADDRESS)
            {
                firewallRule.LocalAddress = FirewallAPI.Rule.ANY_IP_ADDRESS;
            }

            // TODO: See if we can just change the type in the domain model
            if(domainRule.LocalPort == Models.Rule.ANY_PORT)
            {
                firewallRule.LocalPort = FirewallAPI.Rule.ANY_PORT;
            }
            else
            {
                firewallRule.LocalPort = domainRule.LocalPort.Value;
            }

            if(domainRule.RemoteAddress == Models.Rule.ANY_IP_ADDRESS)
            {
                firewallRule.RemoteAddress = FirewallAPI.Rule.ANY_IP_ADDRESS;
            }

            if(domainRule.RemotePort == Models.Rule.ANY_PORT)
            {
                firewallRule.RemotePort = FirewallAPI.Rule.ANY_PORT;
            }
            else
            {
                firewallRule.RemotePort = domainRule.RemotePort.Value;
            }

            Console.WriteLine($"Post-FW rule: {firewallRule}");

            FirewallAPI.FirewallManager.NewRule(firewallRule);
        }

        public static void UpdateRule(Models.Rule domainRule)
        {
            Console.WriteLine($"DOMAIN RULE: {domainRule}");

            var firewallRule = Mapper.Map<FirewallAPI.Rule>(domainRule);

            Console.WriteLine($"Pre-FW RULE: {firewallRule}");

            if(domainRule.LocalAddress == Models.Rule.ANY_IP_ADDRESS)
            {
                firewallRule.LocalAddress = FirewallAPI.Rule.ANY_IP_ADDRESS;
            }

            // TODO: See if we can just change the type in the domain model
            if(domainRule.LocalPort == Models.Rule.ANY_PORT)
            {
                firewallRule.LocalPort = FirewallAPI.Rule.ANY_PORT;
            }
            else
            {
                firewallRule.LocalPort = domainRule.LocalPort.Value;
            }

            if(domainRule.RemoteAddress == Models.Rule.ANY_IP_ADDRESS)
            {
                firewallRule.RemoteAddress = FirewallAPI.Rule.ANY_IP_ADDRESS;
            }

            if(domainRule.RemotePort == Models.Rule.ANY_PORT)
            {
                firewallRule.RemotePort = FirewallAPI.Rule.ANY_PORT;
            }
            else
            {
                firewallRule.RemotePort = domainRule.RemotePort.Value;
            }

            Console.WriteLine($"Post-FW rule: {firewallRule}");

            FirewallAPI.FirewallManager.UpdateRule(firewallRule);
        }

        public static void DeleteRule(uint number)
        {
            FirewallAPI.FirewallManager.DeleteRule(number);
        }
    }
}