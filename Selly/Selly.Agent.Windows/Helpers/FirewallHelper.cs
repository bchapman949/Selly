using AutoMapper;
using Selly.Agent.Windows.FirewallAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selly.Agent.Windows.Helpers
{
    public class FirewallHelper
    {
        public static IList<Windows.Models.Rule> GetRules()
        {
            Policy policy = new Policy();
            var firewallRules = policy.Rules.ToList();
            var domainRules = Mapper.Map<List<Windows.Models.Rule>>(firewallRules);
            return domainRules;
        }

        // Alternative implementation which returns a more filtered
        // list of rules
        public static IList<Windows.Models.Rule> GetRules2()
        {
            Policy policy = new Policy();
            var profile = policy.CurrentProfileTypes;
            var firewallRules = policy.Rules.ToList();
            firewallRules = firewallRules.Where(r =>
                                                r.Direction == RuleDirection.In &&
                                                r.Enabled == true &&
                                                (r.Profiles == (int) profile || r.Profiles == (int) ProfileType.All))
                                         .ToList();

            var dupFreeRules = firewallRules.Distinct(new RuleComparer()).ToList();
            var dups = firewallRules.Except(dupFreeRules);

            var domainRules = Mapper.Map<List<Windows.Models.Rule>>(firewallRules);
            return domainRules;
        }

        public static void NewRule(Windows.Models.Rule rule)
        {
            // There is currently no strongly typed way to do this with
            // the firewall API developed. FirewallAPI.Rule is backed by
            // a COM type which will commit immediately. As such, the individual
            // properties are past instead.

            // TODO: Windows Firewall mapping: This is a bit of a dirty way to do the mapping
            FirewallAPI.Action action = (FirewallAPI.Action) rule.Action;
            int protocol = (int)rule.Protocol;
            FirewallAPI.RuleDirection direction = (FirewallAPI.RuleDirection) rule.Direction;

            Policy policy = new Policy();
            policy.Rules.Add(rule.Name, action, protocol, direction, 
                             rule.LocalAddresses, rule.LocalPorts, rule.RemoteAddresses, rule.RemotePorts);
            
            
            //NewRuleFirewallClient.Execute(rule.Name, (int) rule.Protocol, rule.LocalPorts, action);
        }

        public static void UpdateRule(string name, Windows.Models.Rule updatedRule)
        {
            Policy policy = new Policy();
            var locatedRule = policy.Rules.FirstOrDefault(r => r.Name == name);

            if (locatedRule == null)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "No rule with that name exists");
            }

            /// See <see cref="FirewallAPI.Rule"/> for description of native type
            locatedRule.Name = updatedRule.Name;
            locatedRule.Action = (FirewallAPI.Action) updatedRule.Action;
            locatedRule.Protocol = (int)updatedRule.Protocol;
            locatedRule.Direction = (FirewallAPI.RuleDirection)updatedRule.Direction;
            locatedRule.LocalAddresses = updatedRule.LocalAddresses;
            locatedRule.LocalPorts = updatedRule.LocalPorts;
            locatedRule.RemoteAddresses = updatedRule.RemoteAddresses;
            locatedRule.RemotePorts = updatedRule.RemotePorts;
        }

        public static void DeleteRule(string name)
        {
            Policy policy = new Policy();
            var locatedRule = policy.Rules.FirstOrDefault(r => r.Name == name);

            if (locatedRule == null)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "No rule with that name exists");
            }

            policy.Rules.Remove(name);
        }

        public static bool IsEnabled()
        {
            Policy policy = new Policy();
            return policy.Enabled;
        }

        public static void EnableFirewall()
        {
            Policy policy = new Policy();
            policy.Enabled = true;
        }

        public static void DisableFirewall()
        {
            Policy policy = new Policy();
            policy.Enabled = false;
        }

        private class RuleComparer : IEqualityComparer<Rule>
        {
            public bool Equals(Rule x, Rule y) => x.Name == y.Name;
            public int GetHashCode(Rule obj) => 0;
        }
    }    
}
