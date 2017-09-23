using System;
using System.Net;

namespace Selly.Agent.Linux.Models
{
    public class LinuxRuleAdapter : Generic.Models.Adapter
    {
        Linux.Models.Rule _rule;

        public LinuxRuleAdapter(Linux.Models.Rule rule)
        {
            _rule = rule;
        }

        public LinuxRuleAdapter(Generic.Models.Rule rule)
        {
            _rule = new Linux.Models.Rule();
            
            Name = rule.Name;
            Action = rule.Action;
            Protocol = rule.Protocol;
            Direction = rule.Direction;
            LocalAddress = rule.LocalAddress;
            LocalPort = rule.LocalPort;
            RemoteAddress = rule.RemoteAddress;
            RemotePort = rule.RemotePort;
        }

        // TODO: Fail silently
        public override string FilterID
        {
            get { return null; }
            set { }
        }

        public override string Name
        {
            get { return _rule.Number.ToString(); }

            // TODO: Fail silently
            // Need to allow for null during creation of a rule
            set 
            { 
                uint buffer;
                bool success = uint.TryParse(value, out buffer);
                if(success)
                {
                    _rule.Number = Convert.ToUInt32(value); 
                }
            }
        }

        public override Generic.Models.Action Action
        {
            get
            {
                switch(_rule.Action)
                {
                    case Linux.Models.Action.ALLOW:
                        return Generic.Models.Action.Allow;
                    case Linux.Models.Action.DENY:
                        return Generic.Models.Action.Block;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_rule.Action));
                }
            }
            set
            {
                switch (value)
                {
                    case Generic.Models.Action.Allow:
                        _rule.Action = Linux.Models.Action.ALLOW;
                        break;
                    case Generic.Models.Action.Block:
                        _rule.Action = Linux.Models.Action.DENY;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Action));
                }
            }
        }

        public override Generic.Models.Protocol Protocol
        {
            get
            {
                switch(_rule.Protocol)
                {
                    case Models.Protocol.TCP:
                        return Generic.Models.Protocol.TCP;
                    case Models.Protocol.UDP:
                        return Generic.Models.Protocol.UDP;
                    case Models.Protocol.GRE:
                        return Generic.Models.Protocol.GRE;
                    case Models.Protocol.IGMP:
                        return Generic.Models.Protocol.IGMP;
                    case Models.Protocol.IPv6:
                        return Generic.Models.Protocol.IPv6;
                    case Models.Protocol.ANY:
                        return Generic.Models.Protocol.ANY;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_rule.Protocol));
                }
            }
            set
            {
                switch (value)
                {
                    case Generic.Models.Protocol.TCP:
                        _rule.Protocol = Models.Protocol.TCP;
                        break;
                    case Generic.Models.Protocol.UDP:
                        _rule.Protocol = Models.Protocol.UDP;
                        break;
                    case Generic.Models.Protocol.GRE:
                        _rule.Protocol = Models.Protocol.GRE;
                        break;
                    case Generic.Models.Protocol.IGMP:
                        _rule.Protocol = Models.Protocol.IGMP;
                        break;
                    case Generic.Models.Protocol.IPv6:
                        _rule.Protocol = Models.Protocol.IPv6;
                        break;
                    case Generic.Models.Protocol.ANY:
                        _rule.Protocol = Models.Protocol.ANY;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Protocol));
                }
            }
        }

        // TODO: Fail silently
        public override string LocalAddress
        {
            get 
            { 
                if (_rule.LocalAddress == Linux.Models.Rule.ANY_IP_ADDRESS)
                {
                    return Generic.Models.Rule.ANY_IP_ADDRESS;
                }

                return _rule.LocalAddress;
            }
            set 
            { 
                if (value == Generic.Models.Rule.ANY_IP_ADDRESS)
                {
                    _rule.LocalAddress = Generic.Models.Rule.ANY_IP_ADDRESS;
                }
                else
                {
                    IPAddress buffer;
                    bool success = IPAddress.TryParse(value, out buffer);
                    if(!success) { throw new ArgumentOutOfRangeException(nameof(LocalAddress)); }

                    _rule.LocalAddress = value;
                }
            }
        }

        public override string RemoteAddress
        {
            get
            {
                if(_rule.RemoteAddress == Linux.Models.Rule.ANY_IP_ADDRESS)
                {
                    return Generic.Models.Rule.ANY_IP_ADDRESS;
                }

                return _rule.RemoteAddress;
            }
            set
            {
                if(value == Generic.Models.Rule.ANY_IP_ADDRESS)
                {
                    _rule.RemoteAddress = Linux.Models.Rule.ANY_IP_ADDRESS;
                }
                else
                {
                    IPAddress buffer;
                    bool success = IPAddress.TryParse(value, out buffer);
                    if (!success) { throw new ArgumentOutOfRangeException(nameof(RemoteAddress)); }

                    _rule.RemoteAddress = value;
                }
            }
        }

        // TODO: This will need changing if Linux Firewall API supports multiport values
        public override ushort? LocalPort
        {
            get 
            { 
                if (_rule.LocalPort == Linux.Models.Rule.ANY_PORT) { return Generic.Models.Rule.ANY_PORT; }

                return _rule.LocalPort; 
            }
            set 
            {
                if(!value.HasValue) { throw new ArgumentOutOfRangeException(nameof(LocalPort), "Cannot update rules with unsupported values"); }

                if (value == Generic.Models.Rule.ANY_PORT)
                {
                    _rule.LocalPort = Linux.Models.Rule.ANY_PORT;
                    return;
                }

                _rule.LocalPort = value; 
            }
        }

        // TODO: Fail silently
        public override ushort? RemotePort
        {
            get 
            { 
                if (_rule.RemotePort == Linux.Models.Rule.ANY_PORT) { return Generic.Models.Rule.ANY_PORT; }

                return _rule.RemotePort; 
            }
            set 
            {
                if(!value.HasValue) { throw new ArgumentOutOfRangeException(nameof(RemotePort), "Cannot update rules with unsupported values"); }

                if (value == Generic.Models.Rule.ANY_PORT)
                {
                    _rule.RemotePort = Linux.Models.Rule.ANY_PORT;
                    return;
                }

                _rule.RemotePort = value; 
            }
        }

        public override Generic.Models.Direction Direction 
        { 
            get
            {
                switch (_rule.Direction)
                {
                    case Linux.Models.Direction.IN:
                        return Generic.Models.Direction.In;
                    case Linux.Models.Direction.OUT:
                        return Generic.Models.Direction.Out;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_rule.Direction));
                }
            }
            set
            {
                switch (value)
                {
                    case Generic.Models.Direction.In:
                        _rule.Direction = Linux.Models.Direction.IN;
                        break;
                    case Generic.Models.Direction.Out:
                        _rule.Direction = Linux.Models.Direction.OUT;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Direction));
                }
            }
        }

        public override object NativeType => _rule;
    }
}
