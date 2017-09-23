using System;
using Selly.Agent.API;

namespace Selly.Agent.Windows
{
    internal class ApiConfiguration : IApiConfiguration
    {
        private string _certificatePath = @"C:\Repos\selly-uob\client1.pfx";
        private string _certificatePassword = "selly";
        private string _endpoint = "https://client1:5002";
        private string _appSettingsPath = @"C:\Selly\appsettings";

        public ApiConfiguration()
        {

        }

        public string CertificatePath => _certificatePath;

        public string CertificatePassword => _certificatePassword;

        public string Endpoint => _endpoint;

        public string AppSettingsPath => _appSettingsPath;
    }
}