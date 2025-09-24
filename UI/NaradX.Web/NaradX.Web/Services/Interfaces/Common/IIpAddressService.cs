namespace NaradX.Web.Services.Interfaces.Common
{
    public interface IIpAddressService
    {
        string GetClientIpAddress(HttpContext context);
        string GetClientIpAddress();
    }
}
