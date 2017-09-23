using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Selly.Agent.Linux.FirewallAPI
{
    public class Rule
    {
        public const string ANY_IP_ADDRESS = "any";

        // The UFW parser does not accept 0 or "any".
        // In order to specify any, you must not specify a port
        // clause
        public const ushort ANY_PORT = 0;

        public uint Number {get;set;}
        public FirewallAPI.Action Action {get;set;}
        public Protocol Protocol {get;set;}
        public Direction Direction {get;set;}
        public string LocalAddress{get;set;}
        public ushort LocalPort{get;set;}
        public string RemoteAddress{get;set;}
        public ushort RemotePort {get;set;}

        public override string ToString()
        {
            return $"No: {Number} {Action} {Direction} {Protocol} Local Addr: {LocalAddress} Port: {LocalPort} Remote Addr: {RemoteAddress} Port: {RemotePort}";
        }

        public static List<Rule> Parse(string input)
        {
            List<Rule> rules = new List<Rule>();

            var lines = input.Split('\n');

            for(int i = 4; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) { continue; }

                try
                {
                    var rule = GetRule(lines[i]);
                    if(rule != null)
                    {
                        rules.Add(rule);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to parse rule {e.Message}");
                }
                
            }

            return rules;
        }

        private static Rule GetRule(string line)
        {
            // Skip if a v6 rule
            if(line.Contains("(v6)")) { return null; }

            var rule = new Selly.Agent.Linux.FirewallAPI.Rule();

            // Get rid of output in brackets
            line = line.Replace("(log-all)", "");
            line = line.Replace("(log-all, out)", "");

            // Rule number
            var numbered = line.Split(']');
            rule.Number = Convert.ToUInt32(numbered[0].Replace('[', ' ').Trim());
            //Console.WriteLine($"***{rule.Number}***");

            line = numbered[1].Trim();
            
            if(line.Contains("ALLOW"))
            {
                rule.Action = Action.ALLOW;
            }
            else
            {
                rule.Action = Action.DENY;
            }

            if(line.Contains("IN"))
            {
                rule.Direction = Direction.IN;
            }
            else
            {
                rule.Direction = Direction.OUT;
            }

            string[] combos = line.Split(new string[] {"ALLOW IN", "ALLOW OUT", "DENY IN", "DENY OUT"}, StringSplitOptions.RemoveEmptyEntries);
            combos[0] = combos[0].Trim();
            combos[1] = combos[1].Trim();
            var combo0 = GetCombo(combos[0]);
            var combo1 = GetCombo(combos[1]);

            //Console.WriteLine($"Combo 0: {combo0}");
            //Console.WriteLine($"Combo 1: {combo1}");

            if(rule.Direction == Direction.IN)
            {
                // Local = combo 0
                rule.LocalAddress = combo0.Address;
                rule.LocalPort = combo0.Port;
                
                // Remote = combo 1
                rule.RemoteAddress = combo1.Address;
                rule.RemotePort = combo1.Port;
            }
            else
            {
                // Remote = combo 0
                rule.RemoteAddress = combo0.Address;
                rule.RemotePort = combo0.Port;

                // Local = combo 1
                rule.LocalAddress = combo1.Address;
                rule.LocalPort = combo1.Port;
            }

            rule.Protocol = DetermineProtocol(combo0, combo1);

            //Console.WriteLine(rule.ToString());

            return rule;
        }

        private static Protocol DetermineProtocol(ComboResult combo0, ComboResult combo1)
        {
            if(combo0.Protocol == combo1.Protocol) { return combo0.Protocol; }
            if(combo0.Protocol != Protocol.ANY) { return combo0.Protocol; }
            if(combo1.Protocol != Protocol.ANY) { return combo1.Protocol; }

            return Protocol.ANY;
        }

        private static ComboResult GetCombo(string combo)
        {
            ComboResult result = new ComboResult();

            var tokens = combo.Split('/', ' ');

            foreach (var token in tokens)
            {
                //Console.WriteLine($"'{token}'");
                if(token == "anywhere")
                {
                    result.Address = Rule.ANY_IP_ADDRESS;
                    continue;
                }

                if(token == "tcp")
                {
                    result.Protocol = Protocol.TCP;
                    continue;
                }

                if(token ==  "udp")
                {
                    result.Protocol = Protocol.UDP;
                    continue;
                }

                ushort port;
                if(ushort.TryParse(token, out port))
                {
                    result.Port = port;
                    continue;
                }

                IPAddress address;
                if(IPAddress.TryParse(token, out address))
                {
                    result.Address = address.ToString();
                    continue;
                }
            }

            return result;
        }

        public string Create()
        {
            // http://manpages.ubuntu.com/manpages/precise/man8/ufw.8.html
            
            string action = null;
            switch (Action)
            {
                case Action.ALLOW:
                    action = "allow";
                    break;
                case Action.DENY:
                    action = "deny";
                    break;
                default:
                    throw new Exception();
            }

            string protocol = null;
            switch (Protocol)
            {
                case Protocol.TCP:
                    protocol = "tcp";
                    break;
                case Protocol.UDP:
                    protocol = "udp";
                    break;
                case Protocol.GRE:
                    protocol = "gre";
                    break;
                case Protocol.IPv6:
                    protocol = "ipv6";
                    break;
                case Protocol.IGMP:
                    protocol = "igmp";
                    break;
                case Protocol.ANY:
                    protocol = "any";
                    break;
                default:
                    throw new Exception();
            }

            string direction = null;
            switch (this.Direction)
            {
                case Direction.IN:
                    direction = "in";
                    break;
                case Direction.OUT:
                    direction = "out";
                    break;
                default:
                    throw new Exception();
            }

            string localPort = null;
            if(this.LocalPort == ANY_PORT)
            {
                localPort = "";
            }
            else
            {
                localPort = $"port {this.LocalPort}";
            }

            string remotePort = null;
            if(this.RemotePort == ANY_PORT)
            {
                remotePort = "";
            }
            else
            {
                remotePort = $"port {this.RemotePort}";
            }

            // TODO: Just build the string once dynamically...
            if(this.Direction == FirewallAPI.Direction.IN)
            {
                if(this.Action == Action.DENY)
                {
                    return $"{action} {direction} log-all proto {protocol} from {RemoteAddress} to any {localPort}";
                }
                else
                {
                    return $"{action} {direction} proto {protocol} from {RemoteAddress} to any {localPort}";
                }
            }
            else
            {
                if(this.Action == Action.DENY)
                {
                    return $"{action} {direction} log-all proto {protocol} to {RemoteAddress} {remotePort}";
                }
                else
                {
                    return $"{action} {direction} proto {protocol} to {RemoteAddress} {remotePort}";
                }
            }
        }

        private class ComboResult
        {
            internal ComboResult()
            {
                Address = Rule.ANY_IP_ADDRESS;
                Port = Rule.ANY_PORT;
                Protocol = Protocol.ANY;
            }

            public string Address { get; internal set; }
            public ushort Port { get; internal set; }
            public Protocol Protocol { get; internal set; }

            public override string ToString()
            {
                return $"{Address} {Port} {Protocol}";
            }
        }
    }
}