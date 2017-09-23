// Origin: ASP.NET Core MVC
// It has not been modified in anyway

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Selly.NMS.Web.Models;
using Selly.NMS.Web.Infrastructure;
namespace Selly.NMS.Web.Controllers
{
    public class HomeController : Controller
    {
        IDeviceRepository deviceRepo;
        IEventsRepository eventRepo;

        public HomeController(IDeviceRepository deviceRepository, IEventsRepository eventRepository)
        {
            deviceRepo = deviceRepository;
            eventRepo = eventRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            var events = eventRepo.GetEvents(10);
            var ports = eventRepo.GetViolationsByPortNumber();
            var addresses = eventRepo.GetRemoteIpAddresses();

            ViewData["TopPorts"] = GoogleChartHelpers.ToGoogleChartString(ports, "Port No", "Count");
            ViewData["TopAddresses"] = addresses;

            return View(events);
        }

        public IActionResult Error()
        {
            return RedirectToAction("Index");
            //return View();
        }
    }
}
