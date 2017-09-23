namespace Selly.Agent.Generic.Models
{
    public class Rule : IRule
    {
        public const string ANY_IP_ADDRESS = "ANY";
        public const ushort ANY_PORT = 0;
        public readonly ushort? UNSUPPORTED_PORT = null;

        public string FilterID { get; set; }
        public string Name { get; set; }
        public Action Action { get; set; }
        public Protocol Protocol { get; set; }
        public string LocalAddress { get; set; }
        public ushort? LocalPort { get; set; }
        public string RemoteAddress { get; set; }
        public ushort? RemotePort { get; set; }
        public Direction Direction { get; set; }
    }
}
