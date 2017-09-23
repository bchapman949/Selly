using Selly.NMS.API.DTO;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Selly.Agent.Windows
{
    public class NmsApiClient : IDisposable
    {
        private const string CONTENT_TYPE = "application/json";
        private X509Certificate2 certificate;
        private HttpClient client;

        public NmsApiClient()
        {
            // TODO: Hard coded path
            // Select certificate to send 
            certificate = new X509Certificate2(@"C:\Repos\selly\client1.pfx", "selly");

            // Configure handler
            WebRequestHandler handler = new WebRequestHandler();
            handler.AuthenticationLevel = AuthenticationLevel.MutualAuthRequired;
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckCertificate);
            handler.ClientCertificates.Add(certificate);

            // TODO: Hard coded path
            // HTTP client
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://client2:5002/");
        }

        public async Task SendRequest()
        {
            var content = new StringContent("");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("api/values", content);
        }

        public async Task SendEvent(Event data)
        {
            // Configure content
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE);

            // Make request 
            var response = await client.PostAsync("api/events", content);
            if (!response.IsSuccessStatusCode) { throw new Exception(); }

            // Process reponse
            var jsonResponse = await response.Content.ReadAsStringAsync();
        }

        private static bool CheckCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
