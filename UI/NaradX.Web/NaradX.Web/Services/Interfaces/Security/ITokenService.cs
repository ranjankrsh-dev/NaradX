using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace NaradX.Web.Services.Interfaces.Security
{
    public interface ITokenService
    {
        Task ValidateTokenAsync(CookieValidatePrincipalContext context);
        Task<string> RefreshAccessTokenAsync(string refreshToken);
        bool IsTokenExpired(string token);
        ClaimsPrincipal ValidateJwtToken(string token);
        DateTime GetTokenExpiry(string token);
    }
}
