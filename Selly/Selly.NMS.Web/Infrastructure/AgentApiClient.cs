using Newtonsoft.Json;
using Selly.Agent.API.DTO;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Selly.NMS.Web
{
    public class AgentApiClient
    {
        // Constants
        private const int PORT_NUMBER = 5002;
        private const string HTTPS_SCHEME = "https";
        private const string CONTENT_TYPE = "application/json";
        private const string RULES_ENDPOINT = "api/rules";

        public async Task<EchoResponse> Echo(string host)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Make request
                    var response = await client.GetAsync("api/echo");
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }

                    // Process response
                    var json = await response.Content.ReadAsStringAsync();
                    var echoResponse = JsonConvert.DeserializeObject<EchoResponse>(json);
                    return echoResponse;
                }
            }
        }

        public async Task<GetConfigurationResponse> GetConfiguration(string host)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Make request
                    var response = await client.GetAsync("api/configure");
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }

                    // Process response
                    var json = await response.Content.ReadAsStringAsync();
                    var getConfigurationResponse = JsonConvert.DeserializeObject<GetConfigurationResponse>(json);
                    return getConfigurationResponse;
                }
            }
        }

        public async Task<ConfigureResponse> Configure(string host, ConfigureRequest config)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Configure content
                    var json = JsonConvert.SerializeObject(config);
                    var content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE);

                    // Make request 
                    var response = await client.PostAsync("api/configure", content);
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }

                    // Process reponse
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var configResponse = JsonConvert.DeserializeObject<ConfigureResponse>(jsonResponse);
                    return configResponse;
                }
            }
        }

        public async Task<GetRulesResponse> GetRules(string host)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Make request
                    var response = await client.GetAsync(RULES_ENDPOINT);
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }

                    // Process reponse
                    var json = await response.Content.ReadAsStringAsync();
                    var rules = JsonConvert.DeserializeObject<GetRulesResponse>(json);
                    return rules;
                }
            }
        }

        public async Task SetRulesRequest(string host, SetRulesRequest rules)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Configure content
                    var json = JsonConvert.SerializeObject(rules);
                    var content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE);

                    // Make request
                    var response = await client.PutAsync(RULES_ENDPOINT, content);
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }
                }
            }
        }

        #region Rules

        public async Task NewRule(string host, SetRuleRequest rule)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Configure content
                    var json = JsonConvert.SerializeObject(rule);
                    var content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE);

                    // Make request
                    var response = await client.PostAsync(RULES_ENDPOINT, content);
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }
                }
            }
        }

        public async Task UpdateRule(string host, string ruleName, SetRuleRequest rule)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Configure content
                    var encodedName = Uri.EscapeUriString(ruleName);
                    var json = JsonConvert.SerializeObject(rule);
                    var content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE);

                    // Make request
                    var response = await client.PutAsync($"{RULES_ENDPOINT}/{encodedName}", content);
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }
                }
            }
        }

        public async Task DeleteRule(string host, string name)
        {
            using (var handler = getHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    // Configure client
                    UriBuilder uriBuilder = new UriBuilder(HTTPS_SCHEME, host, PORT_NUMBER);
                    client.BaseAddress = uriBuilder.Uri;

                    // Configure content
                    var encodedName = Uri.EscapeUriString(name);

                    // Make request
                    var response = await client.DeleteAsync($"{RULES_ENDPOINT}/{encodedName}");
                    if (!response.IsSuccessStatusCode) { throw new Exception(); }
                }
            }
        }

        #endregion

        #region Support Functions

        private static HttpClientHandler getHandler()
        {
            var handler = new HttpClientHandler();

            // Server certificate handling
            handler.ServerCertificateCustomValidationCallback = checkCertificate;

            // Client certificate handling
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var certificate = new X509Certificate2(@"C:\Repos\selly\client2.pfx", "selly");
            handler.ClientCertificates.Add(certificate);

            return handler;
        }

        private static bool checkCertificate(HttpRequestMessage sender, X509Certificate2 cert, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion
    }
}