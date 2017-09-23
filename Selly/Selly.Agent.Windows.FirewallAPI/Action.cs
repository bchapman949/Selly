using NetFwTypeLib;

namespace Selly.Agent.Windows.FirewallAPI
{
    /// <summary>
    /// This enumerated type specifies the action for a rule or default setting.
    /// </summary>
    public enum Action 
    {
        /// <summary>
        /// Block traffic. 
        /// </summary>
        Block = NET_FW_ACTION_.NET_FW_ACTION_BLOCK,
        /// <summary>
        /// Allow traffic. 
        /// </summary>
        Allow = NET_FW_ACTION_.NET_FW_ACTION_ALLOW,
        /// <summary>
        /// Maximum traffic. 
        /// </summary>
        Maximum = NET_FW_ACTION_.NET_FW_ACTION_MAX
    }
}
