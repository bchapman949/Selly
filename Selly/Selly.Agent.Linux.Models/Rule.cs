using System;
using System.Collections.Generic;
using System.Linq;

namespace Selly.Agent.Linux.Models
{
    public class Rule
    {
        public const string ANY_IP_ADDRESS = "Anywhere";
        public const ushort ANY_PORT = 0;

        public uint Number {get;set;}
        public Action Action {get;set;}
        public Direction Direction {get;set;}
        public Protocol Protocol {get;set;}
        public string LocalAddress{get;set;}
        // NULL is currently not a valid value for this because the
        // FirewallAPI does not parse multiple port values.
        // 0 should be used for any.
        // NULL may be used in future to indicate unsupported
        // It could also be replaced with another type
        public ushort? LocalPort{get;set;}
        public string RemoteAddress{get;set;}
        public ushort? RemotePort{get;set;}


        public override string ToString()
        {
            return $"No: {Number} {Action} {Direction} {Protocol} Local Addr: {LocalAddress} Port: {LocalPort} Remote Addr: {RemoteAddress} Port: {RemotePort}";
        }
    }
}