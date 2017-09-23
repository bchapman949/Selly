using System.Collections.ObjectModel;
using System.Collections.Generic;
using NetFwTypeLib;
using System;

namespace Selly.Agent.Windows.FirewallAPI
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/aa365345(v=vs.85).aspx
    /// <summary>
    /// The Read only list of  <see cref="Rule"/> objects.
    /// </summary>
    public class Rules : ReadOnlyCollection<Rule>
    {
        // My modification
        INetFwRules _rules;
        // end

        internal Rules(IList<Rule> list)
            : base(list)
        {
        }

        internal Rules(INetFwRules rules)
            : base(RulesToList(rules))
        {
            _rules = rules;
        }

        private static IList<Rule> RulesToList(INetFwRules rules)
        {
            List<Rule> list = new List<Rule>(rules.Count);
            foreach (INetFwRule currentFwRule in rules)
                list.Add(new Rule(currentFwRule));
            return list;
        }

        // My modification
        public void Add(string name, Action action, int protocol, RuleDirection direction, 
                        string localAddresses, string localPorts, string remoteAddresses, string remotePorts)
        {
            Type ruleType = Type.GetTypeFromProgID("HNetCfg.FWRule");
            var fwRule = (INetFwRule)Activator.CreateInstance(ruleType);

            fwRule.Name = name ?? "Generic Rule Name";
            fwRule.Action = (NET_FW_ACTION_)action;
            fwRule.Protocol = protocol;
            fwRule.Direction = (NET_FW_RULE_DIRECTION_)direction;

            fwRule.LocalAddresses = localAddresses;
            fwRule.LocalPorts = localPorts;

            fwRule.RemoteAddresses = remoteAddresses;
            fwRule.RemotePorts = remotePorts;
            
            fwRule.Enabled = true;

            _rules.Add(fwRule);
        }
        // End

        // My modification
        public void Remove(string name)
        {
            _rules.Remove(name);
        }
        // End
    }
}
