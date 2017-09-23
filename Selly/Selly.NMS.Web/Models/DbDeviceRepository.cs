using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Models
{
    public class DbDeviceRepository : IDeviceRepository
    {
        private MainDbContext context;

        public DbDeviceRepository(MainDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Device> Devices
        {
            get
            {
                return context.Devices;
            }
        }

        public async Task AddDevice(Device device)
        {
            context.Devices.Add(device);
            int result = await context.SaveChangesAsync();
        }

        public Device GetDevice(string id)
        {
            return Devices.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task UpdateDevice(Device device)
        {
            await context.SaveChangesAsync();
        }

        public async Task DeleteDevice(Device device)
        {
            // TODO: Improvement: Cascade delete
            context.Events.RemoveRange(context.Events.Where(x => x.DeviceId == device.Id));
            context.Devices.Remove(device);
            int result = await context.SaveChangesAsync();
        }
    }
}
