using NetFwTypeLib;
using System;

namespace Selly.Agent.Windows.FirewallAPI
{
    /// <summary>
    /// This enumerated type specifies which direction of traffic a rule applies to
    /// </summary>
    public enum RuleDirection
    {
        /// <summary>
        /// The rule applies to inbound traffic. 
        /// </summary>
        In = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN,

        /// <summary>
        /// The rule applies to outbound traffic. 
        /// </summary>
        Out = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT,

        /// <summary>
        /// This value is used for boundary checking only and is not valid for application programming. 
        /// </summary>
        [Obsolete("This value is used for boundary checking only and is not valid for application programming.")]
        Max = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_MAX 
    }
}
