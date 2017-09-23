using System;
using static Selly.Agent.Windows.FirewallAPI.Constants;

namespace Selly.Agent.Windows.FirewallAPI
{
    // TODO: Obsolete (remove this)
    // https://msdn.microsoft.com/en-us/library/windows/desktop/aa365293(v=vs.85).aspx
    public class NewRuleFirewallClient
    {
        public static void Execute(string name, int protocol, string localPorts, Action action)
        {
            // Get a reference to the firewall API
            Type FWManagerType = Type.GetTypeFromProgID("HNetCfg.FwMgr");
            dynamic FWManager = Activator.CreateInstance(FWManagerType);

            // Obtain the firewall profile information
            dynamic FWProfile = FWManager.LocalPolicy.CurrentProfile;

            // Create the port rule
            Type PortType = Type.GetTypeFromProgID("HNetCfg.FWOpenPort");

            dynamic newPort = Activator.CreateInstance(PortType);
            newPort.Name = name ?? "Generic Rule Name";
            newPort.Protocol = protocol;
            newPort.Port = localPorts;
            newPort.Action = (int) action;

            // The port entry must also include either a scope or a remote address entry, but not both.
            newPort.Scope = NET_FW_SCOPE_LOCAL_SUBNET;
            //newPort.RemoteAddresses = "10.1.1.1/255.255.255.255";

            newPort.Enabled = true;

            try
            {
                // Try adding the port
                FWProfile.GloballyOpenPorts.Add(newPort);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
