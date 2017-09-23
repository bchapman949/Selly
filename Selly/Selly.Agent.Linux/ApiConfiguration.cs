using System;
using Selly.Agent.API;

namespace Selly.Agent.Linux
{
    internal class ApiConfiguration : IApiConfiguration
    {
        private string _certificatePath = @"/home/user/Repos/selly-uob/kubuntu-vm.pfx";
        private string _certificatePassword = "selly";
        private string _endpoint = "https://kubuntu-vm:5002";
        private string _appSettingsPath = @"/home/user/Repos/selly/Selly/Selly.Agent.Linux/bin/Debug/netcoreapp1.1/appsettings";

        public ApiConfiguration()
        {

        }

        public string CertificatePath => _certificatePath;

        public string CertificatePassword => _certificatePassword;

        public string Endpoint => _endpoint;

        public string AppSettingsPath => _appSettingsPath;
    }
}