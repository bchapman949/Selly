using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Selly.NMS.Web.APICallbacks;

namespace Selly.NMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
            .UseKestrel(options =>
            {
                // TODO: Hard coded path
                options.UseHttps(@"C:\Repos\selly-uob\client2.pfx", "selly");
            })
            // TODO: Hard coded path
            .UseUrls("https://client2:5001")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .UseApplicationInsights()
            .Build();

            var nms = Task.Run(() => host.Run());
            var api = Task.Run(() => API.Program.Main(args, new ApiCallbacks()));
            Task.WhenAll(nms, api).Wait();
        }
    }
}
