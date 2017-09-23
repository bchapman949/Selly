using Microsoft.AspNetCore.Mvc;
using Selly.Agent.API.DTO;

namespace Selly.Agent.API.Controllers
{
    [Route("api/[controller]")]
    public class EchoController : Controller
    {
        [HttpGet]
        public EchoResponse Get()
        {
            return Program.Callbacks.Echo();
        }
    }
}
