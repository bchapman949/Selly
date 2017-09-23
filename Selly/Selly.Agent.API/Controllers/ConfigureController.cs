using Microsoft.AspNetCore.Mvc;
using Selly.Agent.API.DTO;

namespace Selly.Agent.API.Controllers
{
    [Route("api/[controller]")]
    public class ConfigureController : Controller
    {
        [HttpGet]
        public GetConfigurationResponse Get()
        {
            return Program.Callbacks.GetConfiguration();
        }

        [HttpPost]
        public ConfigureResponse Post([FromBody] ConfigureRequest request)
        {
            return Program.Callbacks.Configure(request);
        }
    }
}
