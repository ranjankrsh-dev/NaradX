namespace NaradX.Web.Services.Interfaces.Common
{
    public interface IAuditService
    {
        Task LogLoginAsync(int userId, string ipAddress);
        Task LogLogoutAsync(int userId);
        Task LogAccessDeniedAsync(int userId, string action);
    }
}
