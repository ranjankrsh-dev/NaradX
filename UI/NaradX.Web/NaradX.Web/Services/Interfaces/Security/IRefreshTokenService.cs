namespace NaradX.Web.Services.Interfaces.Security
{
    public interface IRefreshTokenService
    {
        Task<string> RefreshTokenAsync(string refreshToken, string email);
        bool IsRefreshTokenValid(string refreshToken, DateTime expiry);
    }
}
