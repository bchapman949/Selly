using Selly.NMS.API.APICallbacks;
using Selly.NMS.Web.Hubs;
using AutoMapper;
using Selly.NMS.Web.Models;
using System.Linq;
using System;

namespace Selly.NMS.Web.APICallbacks
{
    public class ApiCallbacks : IApiCallbacks
    {
        public void FirewallEvent(string source, API.DTO.PacketDroppedEvent e)
        {
            var domainEvent = Mapper.Map<Models.PacketDroppedEvent>(e);

            // TODO: DB Context: Not from DI
            using (MainDbContext db = new MainDbContext())
            {
                var device = db.Devices.Where(dev => dev.Address == e.LocalAddress).FirstOrDefault();
                domainEvent.Device = device;
                db.Events.Add(domainEvent);
                db.SaveChanges();
            }
            
            try
            {
                HomeHub.instance.Clients.All.messageReceived("System", $@"Event occurred. Port: {e.LocalPort}. Address: {e.RemoteAddress}. FID: {e.FilterName}");
            }
            catch(NullReferenceException ex)
            {

            }
            
        }

        public void PostReceived()
        {
            HomeHub.instance.Clients.All.messageReceived("System", "Server Message");
        }
    }
}
