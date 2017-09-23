using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;

namespace Selly.Agent.API
{
    public class Program
    {
        private static IApiCallbacks _callbacks;
        internal static IApiCallbacks Callbacks => _callbacks;

        private static IApiConfiguration _configuration;
        internal static IApiConfiguration Conf => _configuration;

        public static void Main(string[] args, IApiCallbacks callbacks, IApiConfiguration configuration)
        {
            _callbacks = callbacks;
            _configuration = configuration;

            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    var sslOps = new HttpsConnectionFilterOptions();
                    sslOps.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                    sslOps.ClientCertificateValidation = CheckClientCertificateLogic.CheckClientCertificate;
                    sslOps.ServerCertificate = new X509Certificate2(Conf.CertificatePath, Conf.CertificatePassword);
                    options.UseHttps(sslOps);
                })
                .UseUrls(Conf.Endpoint)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
