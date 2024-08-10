namespace VoucherSystem.ProviderIntegration.Configuration;

public class RabbitMQSettings
{
    public string HostName { get; set; }
    public ushort Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}