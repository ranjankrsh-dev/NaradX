using NaradX.Domain.Entities.ManageContact;
using NaradX.Shared.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Repositories.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<Contact?> GetByIdAsync(int id, int tenantId, CancellationToken cancellationToken = default);
        Task<Contact?> GetByPhoneNumberAsync(string phoneNumber, int tenantId, CancellationToken cancellationToken = default);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber, int tenantId, int? excludeContactId = null, CancellationToken cancellationToken = default);
        Task<PaginatedList<Contact>> GetPaginatedAsync(int tenantId, int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);
    }
}
