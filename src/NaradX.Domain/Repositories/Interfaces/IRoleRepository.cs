using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Role?> GetByTypeAsync(RoleType roleType, CancellationToken cancellationToken = default);

        Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> IsSystemRoleAsync(int roleId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Role>> GetAssignableRolesAsync(CancellationToken cancellationToken = default);

        Task<bool> AssignRoleToUserAsync(int userId, int roleId, CancellationToken cancellationToken = default);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Role>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default);
    }
}
