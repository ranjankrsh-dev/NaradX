using NaradX.Web.Security.Interfaces;

namespace NaradX.Web.Security.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly ILogger<AuditService> _logger;

        public AuditService(ILogger<AuditService> logger)
        {
            _logger = logger;
        }

        public async Task LogLoginAsync(int userId, string ipAddress)
        {
            _logger.LogInformation("User {UserId} logged in from IP {IPAddress}", userId, ipAddress);
            await Task.CompletedTask;
        }

        public async Task LogLogoutAsync(int userId)
        {
            _logger.LogInformation("User {UserId} logged out", userId);
            await Task.CompletedTask;
        }

        public async Task LogAccessDeniedAsync(int userId, string action)
        {
            _logger.LogWarning("User {UserId} was denied access to {Action}", userId, action);
            await Task.CompletedTask;
        }
    }
}
