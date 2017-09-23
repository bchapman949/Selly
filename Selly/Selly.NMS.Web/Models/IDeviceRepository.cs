using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Models
{
    public interface IDeviceRepository
    {
        IEnumerable<Device> Devices { get; }

        Task AddDevice(Device device);
        Device GetDevice(string id);
        Task UpdateDevice(Device device);
        Task DeleteDevice(Device device);
    }
}
