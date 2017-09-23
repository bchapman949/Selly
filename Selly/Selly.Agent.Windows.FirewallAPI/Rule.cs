using NetFwTypeLib;
using System;

namespace Selly.Agent.Windows.FirewallAPI
{
    /// <summary>
    /// Represents a single firewall Rule.
    /// </summary>
    /// <see cref="https://msdn.microsoft.com/en-us/library/windows/desktop/aa365344(v=vs.85).aspx"/>
    public class Rule
    {
        private INetFwRule fwRule;

        // This causes problems when being mapped from the Windows.Rule model
        public Rule()
        {
            Type ruleType = Type.GetTypeFromProgID("HNetCfg.FWRule");
            fwRule = (INetFwRule)Activator.CreateInstance(ruleType);
        }

        /// <summary>
        /// Creates an Instance of the <see>Rule</see> class from a INetFwRule.
        /// </summary>
        /// <param name="FwRule"></param>
        internal Rule(INetFwRule FwRule)
        {
            fwRule = FwRule;
        }

        /// <summary>
        /// Accesses the Action property of this rule. 
        /// </summary>
        public Action Action
        {
            get { return (Action)fwRule.Action; }
            set { fwRule.Action = (NET_FW_ACTION_)value; }
        }
        /// <summary>
        ///  Accesses the ApplicationName property for this rule. 
        /// </summary>
        public string ApplicationName
        {
            get { return fwRule.ApplicationName; }
            set { fwRule.ApplicationName = value; }
        }

        /// <summary>
        /// Accesses the Description property for this rule. 
        /// </summary>
        /// <remarks>
        /// This property is optional. The string must not contain the "|" character.
        /// </remarks>
        public string Description
        {
            get { return fwRule.Description; }
            set { fwRule.Description = value; }
        }

        /// <summary>
        /// Accesses the Direction property for this rule. 
        /// </summary>
        public RuleDirection Direction
        {
            get { return (RuleDirection)fwRule.Direction; }
            set { fwRule.Direction = (NET_FW_RULE_DIRECTION_)value; }
        }
        /// <summary>
        /// Accesses the EdgeTraversal property for this rule. 
        /// </summary>
        public bool EdgeTraversal
        {
            get { return fwRule.EdgeTraversal; }
            set { fwRule.EdgeTraversal = value; }
        }
        /// <summary>
        /// Accesses the Enabled property for this rule. 
        /// </summary>
        public bool Enabled
        {
            get { return fwRule.Enabled; }
            set { fwRule.Enabled = value; }
        }
        /// <summary>
        /// Accesses the Grouping property for this rule. 
        /// </summary>
        public string Grouping
        {
            get { return fwRule.Grouping; }
            set { fwRule.Grouping = value; }
        }
        /// <summary>
        /// Accesses the IcmpTypesAndCodes property for this rule. 
        /// </summary>
        public string IcmpTypesAndCodes
        {
            get { return fwRule.IcmpTypesAndCodes; }
            set { fwRule.IcmpTypesAndCodes = value; }
        }
        /// <summary>
        /// Accesses the Interfaces property for this rule. 
        /// </summary>
        public string[] Interfaces
        {
            get { return (string[])fwRule.Interfaces; }
            set { fwRule.Interfaces = value; }
        }
        /// <summary>
        /// Accesses the InterfaceTypes property for this rule. 
        /// </summary>
        public string InterfaceTypes
        {
            get { return fwRule.InterfaceTypes; }
            set { fwRule.InterfaceTypes = value;}
        }
        /// <summary>
        /// Accesses the LocalAddresses property for this rule. 
        /// </summary>
        public string LocalAddresses
        {
            get { return fwRule.LocalAddresses; }
            set {fwRule.LocalAddresses = value; }
        }
        /// <summary>
        /// Accesses the LocalPorts property of this rule. 
        /// </summary>
        public string LocalPorts
        {
            get { return fwRule.LocalPorts; }
            set { fwRule.LocalPorts = value;}
        }
        /// <summary>
        /// Accesses the Name property for this rule. 
        /// </summary>
        public string Name
        {
            get { return fwRule.Name; }
            set {fwRule.Name = value; }
        }

        /// <summary>
        /// Accesses the Profiles property for this rule. 
        /// </summary>
        public int Profiles
        {
            get {return fwRule.Profiles; }
            set { fwRule.Profiles = value; }
        }
        /// <summary>
        /// Accesses the Protocol property for this rule. 
        /// </summary>
        public int Protocol
        {
            get {return fwRule.Protocol; }
            set { fwRule.Protocol = value; }
        }

        /// <summary>
        /// Accesses the RemoteAddresses property of this rule. 
        /// </summary>
        public string RemoteAddresses
        {
            get {return fwRule.RemoteAddresses; }
            set { fwRule.RemoteAddresses = value; }
        }

        /// <summary>
        /// Accesses the RemotePorts property for this rule. 
        /// </summary>
        public string RemotePorts
        {
            get { return fwRule.RemotePorts; }
            set {fwRule.RemotePorts = value; }
        }
        /// <summary>
        /// Accesses the ServiceaName property for this rule. 
        /// </summary>
        public string ServiceName
        {
            get { return fwRule.serviceName; }
            set { fwRule.serviceName = value; }
        }
    }
}
