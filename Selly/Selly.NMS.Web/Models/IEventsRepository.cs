using System.Collections.Generic;

namespace Selly.NMS.Web.Models
{
    public interface IEventsRepository
    {
        IEnumerable<PacketDroppedEvent> Events { get; }

        PacketDroppedEvent GetEvent(string id);

        ICollection<PacketDroppedEvent> GetEvents();
        ICollection<PacketDroppedEvent> GetEvents(int count);
        ICollection<PacketDroppedEvent> GetEvents(string deviceId);
        ICollection<PacketDroppedEvent> GetEvents(string deviceId, int count);
        ICollection<PacketDroppedEvent> GetEvents(string deviceId, string filterId);
        ICollection<PacketDroppedEvent> GetEvents(string deviceId, string remoteAddress, int localPort);

        int[][] GetEventsForTheLast24Hours(string deviceId);
        int[][] GetEventsForTheLast24Hours(string deviceId, string filterId);
        int[][] GetEventsForTheLast24Hours(string deviceId, string remoteAddress, int localPort);

        string RuleMostTriggeredBy(string deviceId, string filterId);
        string RuleMostTriggeredBy(string deviceId, string remoteAddress, int localPort);

        IDictionary<string, int> GetViolationsByPortNumber();
        IDictionary<string, int> GetRemoteIpAddresses();        
    }
}
