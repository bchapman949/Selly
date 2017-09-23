namespace Selly.Agent.Windows.Models
{
    // NOTE: That the numbers assigned to each enum must match with those
    // in the firewall API enum for a successful mapping

    /// <summary>
    /// This enumerated type specifies the action for a rule or default setting.
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// Block traffic. 
        /// </summary>
        Block = 0,

        /// <summary>
        /// Allow traffic. 
        /// </summary>
        Allow = 1,

        /// <summary>
        /// Maximum traffic. 
        /// </summary>
        Maximum = 2
    }
}
