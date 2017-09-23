using Selly.Agent.Generic.Models;
using System;
using System.Net;

namespace Selly.Agent.Windows.Models
{
    public class WindowsRuleAdapter : Adapter
    {
        Agent.Windows.Models.Rule _rule;

        public WindowsRuleAdapter(Agent.Windows.Models.Rule rule)
        {
            _rule = rule;
        }

        public WindowsRuleAdapter(Agent.Generic.Models.Rule rule)
        {
            _rule = new Agent.Windows.Models.Rule();

            Name = rule.Name;

            Action = rule.Action;
            Direction = rule.Direction;
            Protocol = rule.Protocol;

            LocalAddress = rule.LocalAddress;
            LocalPort = rule.LocalPort;

            RemoteAddress = rule.RemoteAddress;
            RemotePort = rule.RemotePort;
        }

        public override string FilterID { get { return Name; } set { throw new NotImplementedException(); } }

        public override string Name
        {
            get
            {
                return _rule.Name;
            }
            set
            {
                _rule.Name = value;
            }
        }

        public override Agent.Generic.Models.Action Action
        {
            get
            {
                switch (_rule.Action)
                {
                    case Agent.Windows.Models.Action.Block:
                        return Agent.Generic.Models.Action.Block;
                    case Agent.Windows.Models.Action.Allow:
                        return Agent.Generic.Models.Action.Allow;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_rule.Action));
                }
            }
            set
            {
                switch (value)
                {
                    case Agent.Generic.Models.Action.Allow:
                        _rule.Action = Agent.Windows.Models.Action.Allow;
                        break;
                    case Agent.Generic.Models.Action.Block:
                        _rule.Action = Agent.Windows.Models.Action.Block;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Action));
                }
            }
        }

        public override Agent.Generic.Models.Protocol Protocol
        {
            get
            {
                switch (_rule.Protocol)
                {
                    case Agent.Windows.Models.Protocol.TCP:
                        return Agent.Generic.Models.Protocol.TCP;
                    case Agent.Windows.Models.Protocol.UDP:
                        return Agent.Generic.Models.Protocol.UDP;
                    case Agent.Windows.Models.Protocol.GRE:
                        return Agent.Generic.Models.Protocol.GRE;
                    case Agent.Windows.Models.Protocol.IGMP:
                        return Agent.Generic.Models.Protocol.IGMP;
                    case Agent.Windows.Models.Protocol.IPv6:
                        return Agent.Generic.Models.Protocol.IPv6;
                    case Agent.Windows.Models.Protocol.ANY:
                        return Agent.Generic.Models.Protocol.ANY;
                    default:
                        return Generic.Models.Protocol.Other;
                }
            }
            set
            {
                switch (value)
                {
                    case Agent.Generic.Models.Protocol.TCP:
                        _rule.Protocol = Agent.Windows.Models.Protocol.TCP;
                        break;
                    case Agent.Generic.Models.Protocol.UDP:
                        _rule.Protocol = Agent.Windows.Models.Protocol.UDP;
                        break;
                    case Agent.Generic.Models.Protocol.GRE:
                        _rule.Protocol = Agent.Windows.Models.Protocol.GRE;
                        break;
                    case Agent.Generic.Models.Protocol.IGMP:
                        _rule.Protocol = Agent.Windows.Models.Protocol.IGMP;
                        break;
                    case Agent.Generic.Models.Protocol.IPv6:
                        _rule.Protocol = Agent.Windows.Models.Protocol.IPv6;
                        break;
                    case Agent.Generic.Models.Protocol.ANY:
                        _rule.Protocol = Agent.Windows.Models.Protocol.ANY;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_rule.Protocol));
                }
            }
        }

        public override string LocalAddress
        {
            get
            {
                if (_rule.LocalAddresses == Windows.Models.Rule.ANY_IP_ADDRESS)
                {
                    return Generic.Models.Rule.ANY_IP_ADDRESS;
                }

                return _rule.LocalAddresses;
            }
            set
            {
                if (value == Generic.Models.Rule.ANY_IP_ADDRESS)
                {
                    _rule.LocalAddresses = Windows.Models.Rule.ANY_IP_ADDRESS;
                }
                else
                {
                    IPAddress buffer;
                    bool success = IPAddress.TryParse(value, out buffer);
                    if(!success) { throw new ArgumentOutOfRangeException(nameof(LocalAddress)); }

                    _rule.LocalAddresses = value;
                }
            }
        }

        public override string RemoteAddress
        {
            get
            {
                if (_rule.RemoteAddresses == Windows.Models.Rule.ANY_IP_ADDRESS)
                {
                    return Generic.Models.Rule.ANY_IP_ADDRESS;
                }

                return _rule.RemoteAddresses;
            }
            set
            {
                if (value == Generic.Models.Rule.ANY_IP_ADDRESS)
                {
                    _rule.RemoteAddresses = Windows.Models.Rule.ANY_IP_ADDRESS;
                }
                else
                {
                    IPAddress buffer;
                    bool success = IPAddress.TryParse(value, out buffer);
                    if (!success) { throw new ArgumentOutOfRangeException(nameof(RemoteAddress)); }

                    _rule.RemoteAddresses = value;
                }
            }
        }

        /// <summary>
        /// Will be null if there was an error parsing the native value
        /// </summary>
        public override ushort? LocalPort
        {
            get
            {
                if (_rule.LocalPorts == Windows.Models.Rule.ANY_PORT) { return Generic.Models.Rule.ANY_PORT; }

                ushort result = 0;
                bool success = ushort.TryParse(_rule.LocalPorts, out result);

                // TODO: Fail silently - Test 0 as ANY
                if (!success) { return null; } // throw new ArgumentOutOfRangeException(nameof(_rule.LocalPorts)); }
                return result;
            }
            set
            {
                if(!value.HasValue) { throw new ArgumentOutOfRangeException(nameof(LocalPort), "Cannot update rules with unsupported values"); }

                // TODO: Bug?
                if (value == Generic.Models.Rule.ANY_PORT)
                {
                    _rule.RemotePorts = Windows.Models.Rule.ANY_PORT;
                    return;
                }

                _rule.LocalPorts = value.ToString();
            }
        }

        /// <summary>
        /// Can be NULL if not a number, multiple numbers, NULL itself (EG: IPv6)
        /// </summary>
        public override ushort? RemotePort
        {
            get
            {
                // TODO: Fail silently
                if(_rule.RemotePorts == null) { return null; }
                if(_rule.RemotePorts == Windows.Models.Rule.ANY_PORT) { return Generic.Models.Rule.ANY_PORT; }

                ushort result = 0;
                bool success = ushort.TryParse(_rule.RemotePorts, out result);

                // TODO: Fail silently - Test 0 as ANY
                if (!success) { return null; } // throw new ArgumentOutOfRangeException(nameof(_rule.RemotePorts)); }
                return result;
            }
            set
            {
                if (!value.HasValue) { throw new ArgumentOutOfRangeException(nameof(RemotePort), "Cannot update rules with unsupported values"); }

                if (value == Generic.Models.Rule.ANY_PORT)
                {
                    _rule.RemotePorts = Windows.Models.Rule.ANY_PORT;
                    return;
                }

                _rule.RemotePorts = value.ToString();
            }
        }

        public override Direction Direction
        {
            get
            {
                switch (_rule.Direction)
                {
                    case RuleDirection.In:
                        return Direction.In;
                    case RuleDirection.Out:
                        return Direction.Out;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_rule.Direction));
                }
            }
            set
            {
                switch (value)
                {
                    case Direction.In:
                        _rule.Direction = RuleDirection.In;
                        break;
                    case Direction.Out:
                        _rule.Direction = RuleDirection.Out;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Direction));
                }
            }
        }

        public override object NativeType => _rule;
    }
}
