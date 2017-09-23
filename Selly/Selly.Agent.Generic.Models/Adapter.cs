using System;

namespace Selly.Agent.Generic.Models
{
    public abstract class Adapter : IRule
    {
        public abstract object NativeType { get; }

        public abstract string FilterID { get; set; }

        public abstract string Name { get; set; }
        public abstract Action Action { get; set; }
        public abstract Protocol Protocol { get; set; }
        public abstract string LocalAddress { get; set; }
        public abstract ushort? LocalPort { get; set; }
        public abstract string RemoteAddress { get; set; }
        public abstract ushort? RemotePort { get; set; }
        public abstract Direction Direction { get; set; }
    }
}
