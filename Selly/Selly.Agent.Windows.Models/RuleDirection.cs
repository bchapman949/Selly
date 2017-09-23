using System;

namespace Selly.Agent.Windows.Models
{
    /// <summary>
    /// This enumerated type specifies which direction of traffic a rule applies to.
    /// </summary>
    public enum RuleDirection
    {
        /// <summary>
        /// The rule applies to inbound traffic. 
        /// </summary>
        In = 1,

        /// <summary>
        /// The rule applies to outbound traffic. 
        /// </summary>
        Out = 2,

        /// <summary>
        /// This value is used for boundary checking only and is not valid for application programming. 
        /// </summary>
        [Obsolete("This value is used for boundary checking only and is not valid for application programming.")]
        Max = 3
    }
}
