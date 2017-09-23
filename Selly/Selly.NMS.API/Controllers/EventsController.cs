using Microsoft.AspNetCore.Mvc;
using Selly.NMS.API.DTO;
using System.Collections.Generic;

namespace Selly.NMS.API.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        // GET api/events
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/events/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/events
        [HttpPost]
        public void Post([FromBody]PacketDroppedEvent value)
        {
            var source = HttpContext.Connection.RemoteIpAddress.ToString();
            Program.Callbacks.FirewallEvent(source, value);
        }

        // PUT api/events/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/events/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
