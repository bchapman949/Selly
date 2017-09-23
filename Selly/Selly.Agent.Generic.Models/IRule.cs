namespace Selly.Agent.Generic.Models
{
    public interface IRule
    {
        string FilterID { get; }
        string Name { get; set; }
        Action Action { get; set; }
        Protocol Protocol { get; set; }

        string LocalAddress { get; set; }
        ushort? LocalPort { get; set; }
        string RemoteAddress { get; set; }
        ushort? RemotePort { get; set; }
        Direction Direction { get; set; }
    }
}
