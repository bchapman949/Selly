namespace Selly.Agent.Linux.FirewallAPI
{
    public enum Protocol 
    {
        TCP,
        UDP,
        GRE,
        IPv6,
        IGMP,
        ANY // Corresponds to TCP and UDP. See http://manpages.ubuntu.com/manpages/xenial/en/man8/ufw.8.html
    }
}