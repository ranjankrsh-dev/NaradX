namespace NaradX.Web.Security.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> RefreshTokenAsync(string refreshToken, string email);
        bool IsRefreshTokenValid(string refreshToken, DateTime expiry);
    }
}
