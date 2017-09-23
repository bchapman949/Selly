using System;

namespace Selly.Agent.Windows.Models
{
    public class Rule
    {
        /// <summary>
        /// The string literal used to define any IP address
        /// </summary>
        public const string ANY_IP_ADDRESS = "*";

        /// <summary>
        /// The string literal used to define any port number
        /// </summary>
        public const string ANY_PORT = "*";

        /// <summary>
        /// Accesses the Action property of this rule. 
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        ///  Accesses the ApplicationName property for this rule. 
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Accesses the Description property for this rule. 
        /// </summary>
        /// <remarks>
        /// This property is optional. The string must not contain the "|" character.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Accesses the Direction property for this rule. 
        /// </summary>
        public RuleDirection Direction { get; set; }

        /// <summary>
        /// Accesses the EdgeTraversal property for this rule. 
        /// </summary>
        public bool EdgeTraversal { get; set; }

        /// <summary>
        /// Accesses the Enabled property for this rule. 
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Accesses the Grouping property for this rule. 
        /// </summary>
        public string Grouping { get; set; }

        /// <summary>
        /// Accesses the IcmpTypesAndCodes property for this rule. 
        /// </summary>
        public string IcmpTypesAndCodes { get; set; }

        /// <summary>
        /// Accesses the Interfaces property for this rule. 
        /// </summary>
        public string[] Interfaces { get; set; }

        /// <summary>
        /// Accesses the InterfaceTypes property for this rule. 
        /// </summary>
        public string InterfaceTypes { get; set; }

        /// <summary>
        /// Accesses the LocalAddresses property for this rule. 
        /// </summary>
        public string LocalAddresses { get; set; }

        /// <summary>
        /// Accesses the LocalPorts property of this rule. 
        /// </summary>
        public string LocalPorts { get; set; }

        /// <summary>
        /// Accesses the Name property for this rule. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Accesses the Profiles property for this rule. 
        /// </summary>
        public int Profiles { get; set; }

        /// <summary>
        /// Accesses the Protocol property for this rule. 
        /// </summary>
        public Protocol Protocol { get; set; }

        /// <summary>
        /// Accesses the RemoteAddresses property of this rule. 
        /// </summary>
        public string RemoteAddresses { get; set; }

        /// <summary>
        /// Accesses the RemotePorts property for this rule. 
        /// </summary>
        public string RemotePorts { get; set; }

        /// <summary>
        /// Accesses the ServiceaName property for this rule. 
        /// </summary>
        public string ServiceName { get; set; }
    }
}
