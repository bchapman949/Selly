using Selly.NMS.Web.Models;
using System.Collections.Generic;

namespace Selly.NMS.Web.ViewModels.Events
{
    public class IndexVM
    {
        public IEnumerable<PacketDroppedEvent> Events { get; set; }
        public Models.Device Device { get; set; }
    }
}
