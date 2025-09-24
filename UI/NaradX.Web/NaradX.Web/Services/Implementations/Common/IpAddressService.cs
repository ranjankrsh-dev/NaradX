using NaradX.Web.Services.Interfaces.Common;

namespace NaradX.Web.Services.Implementations.Common
{
    public class IpAddressService : IIpAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IpAddressService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetClientIpAddress()
        {
            return GetClientIpAddress(_httpContextAccessor.HttpContext);
        }

        public string GetClientIpAddress(HttpContext context)
        {
            if (context == null) return "unknown";

            try
            {
                // Common headers for proxy scenarios
                var headers = new[] { "X-Forwarded-For", "X-Real-IP", "CF-Connecting-IP", "True-Client-IP" };

                foreach (var header in headers)
                {
                    var ip = context.Request.Headers[header].FirstOrDefault();
                    if (!string.IsNullOrEmpty(ip))
                    {
                        // Handle multiple IPs (e.g., "client, proxy1, proxy2")
                        var ips = ip.Split(',');
                        var clientIp = ips[0].Trim();

                        if (IsValidIpAddress(clientIp))
                        {
                            return clientIp;
                        }
                    }
                }

                // Fall back to remote IP
                var remoteIp = context.Connection.RemoteIpAddress?.ToString();
                if (IsValidIpAddress(remoteIp))
                {
                    return remoteIp;
                }

                return "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        private bool IsValidIpAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;

            // Check for localhost addresses
            if (ip == "::1" || ip == "127.0.0.1" || ip == "localhost")
                return true;

            // Simple IP validation
            return System.Net.IPAddress.TryParse(ip, out _);
        }
    }
}
