namespace NaradX.Infrastructure.Gateways.WhatsApp;

public class WhatsAppOptions
{
    public string BaseUrl { get; set; } = "https://graph.facebook.com";
    public string BusinessId { get; set; }
    public string AccessToken { get; set; }
}
