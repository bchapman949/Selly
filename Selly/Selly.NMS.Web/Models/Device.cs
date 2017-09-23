using System.Collections.Generic;

namespace Selly.NMS.Web.Models
{
    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public List<PacketDroppedEvent> Events { get; set; }
    }
}
