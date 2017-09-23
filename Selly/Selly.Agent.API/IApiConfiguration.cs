namespace Selly.Agent.API
{
    public interface IApiConfiguration
    {
        string CertificatePath { get; }
        string CertificatePassword { get; }
        string Endpoint { get; }
        string AppSettingsPath { get; }
    }
}