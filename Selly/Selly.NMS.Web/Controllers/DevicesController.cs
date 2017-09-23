using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selly.Agent.API.DTO;
using Selly.NMS.Web.Infrastructure;
using Selly.NMS.Web.Models;
using Selly.NMS.Web.ViewModels.Device;
using System.Linq;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        IDeviceRepository deviceRepo;
        IEventsRepository eventsRepo;

        public DevicesController(IDeviceRepository deviceRepository, IEventsRepository eventsRepository)
        {
            deviceRepo = deviceRepository;
            eventsRepo = eventsRepository;
        }

        public IActionResult Index()
        {
            return View(deviceRepo.Devices);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Device model)
        {
            if (ModelState.IsValid)
            {
                await deviceRepo.AddDevice(model);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            var device = deviceRepo.GetDevice(id);
            return View(device);
        }

        public async new Task<IActionResult> View(string id)
        {
            Device device = deviceRepo.GetDevice(id);
            var events = eventsRepo.GetEvents(device.Id, 5);

            ViewVM viewModel = new ViewVM();
            viewModel.Device = device;
            viewModel.Events = events;

            AgentApiClient client = new AgentApiClient();
            try
            {
                var response = await client.GetConfiguration(device.Address);

                var hoursWithActivity = eventsRepo.GetEventsForTheLast24Hours(device.Id);
                var last24Hours = GoogleChartHelpers.To24HourArray(hoursWithActivity);
                ViewData["Last24Hours"] = GoogleChartHelpers.ToGoogleChartString(last24Hours, "Hour", "Count");

                viewModel.Online = true;
                viewModel.FirewallEnabled = response.FirewallEnabled;
            }
            catch (System.Net.Http.HttpRequestException)
            {
                viewModel.Online = false;
            }
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Device updatedDevice)
        {
            Device device = deviceRepo.Devices.FirstOrDefault(dev => dev.Id == updatedDevice.Id);
            
            if (device != null)
            {
                device.Address = updatedDevice.Address;
                device.Name = updatedDevice.Name;
                await deviceRepo.UpdateDevice(device);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Device Not Found");
            }

            return View(updatedDevice);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var device = deviceRepo.GetDevice(id);

            if (device != null)
            {
                await deviceRepo.DeleteDevice(device);
            }
            else
            {
                ModelState.AddModelError("", "Device Not Found");
            }

            return View("Index", deviceRepo.Devices);
        }


        public async Task<IActionResult> Enable(string id)
        {
            var device = deviceRepo.GetDevice(id);
            var client = new AgentApiClient();
            var response = await client.Configure(device.Address, new ConfigureRequest() { FirewallEnabled = true });
            return RedirectToAction("View", new { id=id });
        }

        public async Task<IActionResult> Disable(string id)
        {
            var device = deviceRepo.GetDevice(id);
            var client = new AgentApiClient();
            var response = await client.Configure(device.Address, new ConfigureRequest() { FirewallEnabled = false });
            return RedirectToAction("View", new { id = id });
        }





        public async Task<IActionResult> Events(string id)
        {
            var device = deviceRepo.Devices.FirstOrDefault(dev => dev.Id == id);
            var e = device.Events;

            return RedirectToAction("Index");
        }
    }
}
