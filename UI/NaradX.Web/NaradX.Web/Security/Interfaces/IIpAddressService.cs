namespace NaradX.Web.Security.Interfaces
{
    public interface IIpAddressService
    {
        string GetClientIpAddress(HttpContext context);
        string GetClientIpAddress();
    }
}
