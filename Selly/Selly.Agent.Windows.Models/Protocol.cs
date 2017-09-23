namespace Selly.Agent.Windows.Models
{
    /// <summary>
    /// Protocol. NOTE: These values must match those used by the Windows Firewall API.
    /// The mapping between the 'Firewall model' Protocol and the 'Windows model' Protocol
    /// is simply a simple cast.
    /// </summary>
    public enum Protocol
    {
        TCP = 6,
        UDP = 17,
        GRE = 47,
        IPv6 = 41,
        IGMP = 2,
        ANY = 256,
        ICMPv4 = 1,
        ICMPv6 = 58        
    }
}
