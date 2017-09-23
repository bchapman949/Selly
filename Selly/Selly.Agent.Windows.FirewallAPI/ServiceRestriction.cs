using NetFwTypeLib;

namespace Selly.Agent.Windows.FirewallAPI
{
    public class ServiceRestriction
    {
        INetFwServiceRestriction fwServiceRestriction;

        internal ServiceRestriction(INetFwServiceRestriction serviceRestriction)
        {
            fwServiceRestriction =  serviceRestriction;
        }

        public Rules Rules
        {
            get { return new Rules(fwServiceRestriction.Rules); }
        }

        public bool ServiceRestricted(string ServiceName, string AppName)
        {
            return fwServiceRestriction.ServiceRestricted(ServiceName, AppName);
        }
    }
}
