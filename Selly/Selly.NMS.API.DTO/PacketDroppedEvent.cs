namespace Selly.NMS.API.DTO
{
    public class PacketDroppedEvent : Event
    {
        public string LocalAddress { get; set; }
        public ushort LocalPort { get; set; }

        public string RemoteAddress { get; set; }
        public ushort RemotePort { get; set; }

        public string FilterName { get; set; }

        public override string ToString()
        {
            return $"{Time.ToString()} {RemoteAddress} {RemotePort} {LocalAddress} {LocalPort}";
        }
    }
}
