using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Selly.Agent.API
{
    public class CheckClientCertificateLogic
    {
        public static bool CheckClientCertificate(X509Certificate2 a, X509Chain b, SslPolicyErrors c)
        {
            return true;
        }
    }
}
