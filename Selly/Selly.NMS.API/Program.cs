using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;
using Selly.NMS.API.APICallbacks;

namespace Selly.NMS.API
{
    public class Program
    {
        public static IApiCallbacks Callbacks;

        public static void Main(string[] args, IApiCallbacks callbacks)
        {
            Callbacks = callbacks;

            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    var sslOps = new HttpsConnectionFilterOptions();
                    sslOps.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                    sslOps.ClientCertificateValidation = CheckClientCertificateLogic.CheckClientCertificate;
                    sslOps.ServerCertificate = new X509Certificate2(@"C:\Repos\selly-uob\client2.pfx", "selly");

                    options.UseHttps(sslOps);
                })
                .UseUrls("https://client2:5002")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
