using System;

namespace Selly.Agent.Windows.FirewallAPI
{
    public static class Constants
    {
        // Define constants from the SDK
        // Scope
        public const Int32 NET_FW_SCOPE_ALL = 0;
        public const String NET_FW_SCOPE_ALL_NAME = "All subnets";

        public const Int32 NET_FW_SCOPE_LOCAL_SUBNET = 1;
        public const String NET_FW_SCOPE_LOCAL_SUBNET_NAME = "Local subnet only";

        public const Int32 NET_FW_SCOPE_CUSTOM = 2;
        public const String NET_FW_SCOPE_CUSTOM_NAME = "Custom Scope(see RemoteAddresses)";

        //Profile Type
        public const Int32 NET_FW_PROFILE_DOMAIN = 0;
        public const String NET_FW_PROFILE_DOMAIN_NAME = "Domain";

        public const Int32 NET_FW_PROFILE_STANDARD = 1;
        public const String NET_FW_PROFILE_STANDARD_NAME = "Standard";

        //IP Version
        public const Int32 NET_FW_IP_VERSION_V4 = 0;
        public const String NET_FW_IP_VERSION_V4_NAME = "IPv4";

        public const Int32 NET_FW_IP_VERSION_V6 = 1;
        public const String NET_FW_IP_VERSION_V6_NAME = "IPv6";

        public const Int32 NET_FW_IP_VERSION_ANY = 2;
        public const String NET_FW_IP_VERSION_ANY_NAME = "ANY";

        //Protocol
        public const Int32 NET_FW_IP_PROTOCOL_TCP = 6;
        public const String NET_FW_IP_PROTOCOL_TCP_NAME = "TCP";

        public const Int32 NET_FW_IP_PROTOCOL_UDP = 17;
        public const String NET_FW_IP_PROTOCOL_UDP_NAME = "UDP";

        public const Int32 NET_FW_IP_PROTOCOL_ICMPv4 = 1;
        public const String NET_FW_IP_PROTOCOL_ICMPv4_NAME = "ICMPv4";

        public const Int32 NET_FW_IP_PROTOCOL_ICMPv6 = 58;
        public const String NET_FW_IP_PROTOCOL_ICMPv6_NAME = "ICMPv6";

        public const Int32 NET_FW_IP_PROTOCOL_GRE = 47;
        public const String NET_FW_IP_PROTOCOL_GRE_NAME = "GRE";

        public const Int32 NET_FW_IP_PROTOCOL_IGMP = 2;
        public const String NET_FW_IP_PROTOCOL_IGMP_NAME = "IGMP";

        public const Int32 NET_FW_IP_PROTOCOL_IPv6 = 41;
        public const String NET_FW_IP_PROTOCOL_IPv6_NAME = "IPv6";

        public const Int32 NET_FW_IP_PROTOCOL_ANY = 256;
        public const String NET_FW_IP_PROTOCOL_ANY_NAME = "ANY";
    }
}
