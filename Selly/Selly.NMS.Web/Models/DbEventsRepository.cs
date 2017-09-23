using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selly.NMS.Web.Models
{
    public class DbEventsRepository : IEventsRepository
    {
        private MainDbContext context;

        public DbEventsRepository(MainDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<PacketDroppedEvent> Events
        {
            get
            {
                return context.Events;
            }
        }

        public ICollection<PacketDroppedEvent> GetEvents()
        {
            return context.Events.Include(x => x.Device)
                                 .ToList();
        }

        public ICollection<PacketDroppedEvent> GetEvents(int count)
        {
            return context.Events.OrderByDescending(x => x.Time)
                                 .Take(count)
                                 .Include(x => x.Device)
                                 .ToList();
        }

        public ICollection<PacketDroppedEvent> GetEvents(string deviceId, string filterId)
        {
            return context.Events.Where(x => x.DeviceId == deviceId && x.FilterName == filterId)
                                 .OrderByDescending(x => x.Time)
                                 .ToList();
        }

        public int[][] GetEventsForTheLast24Hours(string deviceId)
        {
            return context.Events.Where(x => x.DeviceId == deviceId)
                                 .Where(x => x.Time > DateTimeOffset.Now.Subtract(TimeSpan.FromHours(23)))
                                 .OrderBy(x => x.Time)
                                 .GroupBy(x => x.Time.ToLocalTime().Hour)
                                 .Select(x => new int[2] { x.Key, x.Count()})
                                 .ToArray();
        }

        public int[][] GetEventsForTheLast24Hours(string deviceId, string filterId)
        {
            return context.Events.Where(x => x.DeviceId == deviceId)
                                 .Where(x => x.FilterName == filterId)
                                 .Where(x => x.Time > DateTimeOffset.Now.Subtract(TimeSpan.FromHours(23)))
                                 .OrderBy(x => x.Time)
                                 .GroupBy(x => x.Time.ToLocalTime().Hour)
                                 .Select(x => new int[2] { x.Key, x.Count() })
                                 .ToArray();
        }

        public int[][] GetEventsForTheLast24Hours(string deviceId, string remoteAddress, int localPort)
        {
            if (remoteAddress == Selly.Agent.Generic.Models.Rule.ANY_IP_ADDRESS)
            {
                return context.Events.Where(x => x.DeviceId == deviceId)
                                 .Where(x => x.LocalPort == localPort)
                                 .Where(x => x.Time > DateTimeOffset.Now.Subtract(TimeSpan.FromHours(23)))
                                 .OrderBy(x => x.Time)
                                 .GroupBy(x => x.Time.ToLocalTime().Hour)
                                 .Select(x => new int[2] { x.Key, x.Count() })
                                 .ToArray();
            }
            else
            {
                return context.Events.Where(x => x.DeviceId == deviceId)
                                 .Where(x => x.RemoteAddress == remoteAddress)
                                 .Where(x => x.LocalPort == localPort)
                                 .Where(x => x.Time > DateTimeOffset.Now.Subtract(TimeSpan.FromHours(23)))
                                 .OrderBy(x => x.Time)
                                 .GroupBy(x => x.Time.ToLocalTime().Hour)
                                 .Select(x => new int[2] { x.Key, x.Count() })
                                 .ToArray();
            }
        }
        
        public string RuleMostTriggeredBy(string deviceId, string filterId)
        {
            var data = context.Events.Where(x => x.DeviceId == deviceId)
                                     .Where(x => x.FilterName == filterId)
                                     .GroupBy(x => x.RemoteAddress)
                                     .OrderByDescending(x => x.Count())
                                     .ToList();

            if (data.Count == 0)
            {
                return null;
            }

            return data.First().Key;
        }

        public string RuleMostTriggeredBy(string deviceId, string remoteAddress, int localPort)
        {
            List<IGrouping<string, PacketDroppedEvent>> data;

            if(remoteAddress == Selly.Agent.Generic.Models.Rule.ANY_IP_ADDRESS)
            {
                data = context.Events.Where(x => x.DeviceId == deviceId)
                                     .Where(x => x.LocalPort == localPort)
                                     .GroupBy(x => x.RemoteAddress)
                                     .OrderByDescending(x => x.Count())
                                     .ToList();
            }
            else
            {
                data = context.Events.Where(x => x.DeviceId == deviceId)
                                     .Where(x => x.RemoteAddress == remoteAddress)
                                     .Where(x => x.LocalPort == localPort)
                                     .GroupBy(x => x.RemoteAddress)
                                     .OrderByDescending(x => x.Count())
                                     .ToList();
            }

            if(data.Count == 0)
            {
                return null;
            }

            return data.First().Key;
        }

        public ICollection<PacketDroppedEvent> GetEvents(string deviceId)
        {
            return context.Events.Where(ev => ev.DeviceId == deviceId)
                                 .OrderByDescending(ev => ev.Time)
                                 .ToList();
        }

        public ICollection<PacketDroppedEvent> GetEvents(string deviceId, int count)
        {
            if(count < 1) { throw new ArgumentOutOfRangeException(nameof(count)); }

            return context.Events.Where(ev => ev.DeviceId == deviceId)
                                 .OrderByDescending(ev => ev.Time)
                                 .Take(count)
                                 .ToList();
        }

        public IDictionary<string, int> GetViolationsByPortNumber()
        {
            return context.Events.GroupBy(x => x.LocalPort)
                                 .OrderByDescending(x => x.Count())
                                 .Take(5)
                                 .ToDictionary(x => x.Key.ToString(), x => x.Count());
        }

        public IDictionary<string, int> GetRemoteIpAddresses()
        {
            return context.Events.GroupBy(x => x.RemoteAddress)
                                 .OrderByDescending(x => x.Count())
                                 .Take(5)
                                 .ToDictionary(x => x.Key.ToString(), x => x.Count());
        }

        public ICollection<PacketDroppedEvent> GetEvents(string deviceId, string remoteAddress, int localPort)
        {
            // Start with broadest search
            var result = context.Events.Where(x => x.DeviceId == deviceId && x.LocalPort == localPort)
                                       .OrderByDescending(x => x.Time)
                                       .ToList();
            
            // If no results, return empty list
            if (result.Count <= 1) { return result; }

            // Narrow search
            var improved = context.Events.Where(x => x.DeviceId == deviceId && x.LocalPort == localPort && x.RemoteAddress == remoteAddress)
                                         .OrderByDescending(x => x.Time)
                                         .ToList();

            if (improved.Count <=1) { return result; }

            return improved;
        }

        public PacketDroppedEvent GetEvent(string id)
        {
            return Events.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
