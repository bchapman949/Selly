using AutoMapper;
using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Selly.Agent.Linux
{
    class SellyService
    {
        const string name = "Selly Service";
        Helpers.NetfilterLogHelper netfilterLogHelper;
        Helpers.UfwLogHelper ufwLogHelper;
        SnmpClient snmpClient;

        public SellyService()
        {
            Mapper.Initialize((mappings) =>
            {
                mappings.CreateMap<FirewallAPI.Rule, Models.Rule>().ReverseMap();
            });

            netfilterLogHelper = new Helpers.NetfilterLogHelper();
            ufwLogHelper = new Helpers.UfwLogHelper();
            snmpClient = new SnmpClient();
        }

        public void OnStart(string[] args)
        {
            try
            {
                ToastHelper.PopToast("Starting...");

                Console.WriteLine("Starting API");
                var api = Task.Run(delegate { API.Program.Main(args, new ApiCallbacks(), new ApiConfiguration()); });

                Console.WriteLine("Starting Netfilter Listener");
                var logHelper = Task.Run(delegate { netfilterLogHelper.Start(); });

                Console.WriteLine("Starting UFW Listener");
                var ufwLogHelperTask = Task.Run(delegate { ufwLogHelper.Start(); });

                Task.Run(delegate { snmpClient.Start(); });

                Task.WhenAll(api, logHelper, ufwLogHelperTask).Wait();
            }
            catch (Exception e) 
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(e.Message);
                sb.AppendLine("\n\n");
                sb.AppendLine(e.StackTrace);

                if(e.InnerException != null)
                {
                    sb.AppendLine("\n\n\n\n");
                    sb.AppendLine(e.InnerException.Message);
                    sb.AppendLine("\n\n");
                    sb.AppendLine(e.InnerException.StackTrace);
                }

                ExceptionHelper.WriteFile("Global", sb.ToString());
            }            
        }

        public void OnStop()
        {
            ToastHelper.PopToast("Stopping...");

            try
            {
                netfilterLogHelper.Stop();
                ufwLogHelper.Stop();
                snmpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{name}: ERROR stopping: {ex.Message}");
                ExceptionHelper.WriteFile(name, ex.Message);
            }
        }
    }
}
