using Selly.Agent.Generic.Models;
using Selly.NMS.Web.Models;
using System.Collections.Generic;

namespace Selly.NMS.Web.ViewModels.Device
{
    public class ViewVM
    {
        public Models.Device Device { get; set; }
        public IEnumerable<PacketDroppedEvent> Events { get; set; }
        public IEnumerable<IRule> Rules { get; set; }
        public bool Online { get; set; }
        public bool FirewallEnabled { get; set; }
    }
}
