using System;

namespace Selly.NMS.Web.Models
{
    public class PacketDroppedEvent
    {
        public string Id { get; set; }

        public string DeviceId { get; set; }
        public Device Device { get; set; }


        public DateTimeOffset Time { get; set; }

        public string LocalAddress { get; set; }
        public int LocalPort { get; set; }

        public string RemoteAddress { get; set; }
        public int RemotePort { get; set; }

        public string FilterName { get; set; }
    }
}
