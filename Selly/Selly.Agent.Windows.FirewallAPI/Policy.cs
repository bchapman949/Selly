using NetFwTypeLib;
using System;

namespace Selly.Agent.Windows.FirewallAPI
{
    /// <summary>
    /// This class enables a read-only access to most of properties of firewall policy under Windows Vista
    /// </summary>
    /// <remarks>
    /// The Windows Firewall/Internet Connection Sharing service must be running to access this class.
    /// This class requires Windows Vista.
    /// </remarks>
    public partial class Policy
    {
        /// <summary>
        /// Contains an instance of the "HNetCfg.FwPolicy2" object.
        /// </summary>
        private INetFwPolicy2 fwPolicy2;

        /// <summary>
        /// Contains currently active profile type. All properies will be read for this profile types.
        /// </summary>
        private NET_FW_PROFILE_TYPE2_ fwCurrentProfileTypes;

        /// <summary>
        /// Creates an instance of <see cref="Policy"/> object.
        /// </summary>
        public Policy()
        {
            //Create an instance of "HNetCfg.FwPolicy2"
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
            //read Current Profile Types (only to increase Performace)
            //avoids access on CurrentProfileTypes from each Property
            fwCurrentProfileTypes = (NET_FW_PROFILE_TYPE2_)fwPolicy2.CurrentProfileTypes;
        }

        /// <summary>
        /// Indicates whether the firewall is enabled.
        /// </summary>
        /// <exception cref=""></exception>
        public bool Enabled
        {
            get {return fwPolicy2.get_FirewallEnabled(fwCurrentProfileTypes);}
            //My modification
            set { fwPolicy2.set_FirewallEnabled(fwCurrentProfileTypes, value); }
            //End my modification
        }

        /// <summary>
        /// The read only list of Rules.
        /// </summary>
        public Rules Rules
        {
            get {return new Rules(fwPolicy2.Rules);}
        }


        /// <summary>
        /// Indicates that inbound traffic should be blocked by the firewall. 
        /// </summary>
        public bool BlockAllInboundTraffic
        {
            get { return fwPolicy2.get_BlockAllInboundTraffic(fwCurrentProfileTypes); }
        }
        /// <summary>
        /// Retrieves currently active profiles. 
        /// </summary>
        public ProfileType CurrentProfileTypes
        {
            get { return (ProfileType)fwPolicy2.CurrentProfileTypes; }
        }

        /// <summary>
        /// Specifies the default action for inbound traffic. 
        /// </summary>
        public Action DefaultInboundAction
        {
            get { return (Action)fwPolicy2.get_DefaultInboundAction(fwCurrentProfileTypes); }
        }

        /// <summary>
        /// Specifies the default action for outbound. 
        /// </summary>
        public Action DefaultOutboundAction
        {
            get { return (Action)fwPolicy2.get_DefaultOutboundAction(fwCurrentProfileTypes); }
        }

        /// <summary>
        /// A list of interfaces on which firewall settings are excluded. 
        /// </summary>
        public string[] ExcludedInterfaces
        {
            get {return (string[])fwPolicy2.get_ExcludedInterfaces(fwCurrentProfileTypes);}
        }

        /// <summary>
        /// Indicates whether interactive firewall notifications are disabled. 
        /// </summary>
        public bool NotificationsDisabled
        {
            get { return fwPolicy2.get_NotificationsDisabled(fwCurrentProfileTypes); }
        }

        /// <summary>
        /// Access to the Windows Service Hardening (WSH) store. 
        /// </summary>
        public ServiceRestriction ServiceRestriction
        {
            get { return new ServiceRestriction(fwPolicy2.ServiceRestriction); }
        }

        /// <summary>
        /// Indicates whether unicast incoming responses to outgoing multicast and broadcast traffic are disabled. 
        /// </summary>
        public bool UnicastResponsesToMulticastBroadcastDisabled
        {
            get { return fwPolicy2.get_UnicastResponsesToMulticastBroadcastDisabled(fwCurrentProfileTypes); }
        }
    }
}