using NaradX.Web.Services.Interfaces.Security;

namespace NaradX.Web.Services.Implementations.Security
{
    public class RefreshTokenService : IRefreshTokenService
    {
        public bool IsRefreshTokenValid(string refreshToken, DateTime expiry)
        {
            return !string.IsNullOrEmpty(refreshToken) && expiry > DateTime.UtcNow;
        }

        public async Task<string> RefreshTokenAsync(string refreshToken, string email)
        {
            // This would typically call your API's refresh token endpoint
            // For now, we'll handle this in the TokenService
            return await Task.FromResult<string>(null);
        }
    }
}
