using Microsoft.EntityFrameworkCore;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(NaradXDbContext context) : base(context)
        {
        }

        public async Task<Contact?> GetByIdAsync(int id, int tenantId, CancellationToken cancellationToken = default)
        {
            return await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId && !c.IsDeleted, cancellationToken);
        }

        public async Task<Contact?> GetByPhoneNumberAsync(string phoneNumber, int tenantId, CancellationToken cancellationToken = default)
        {
            return await _context.Contacts
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.TenantId == tenantId && !c.IsDeleted, cancellationToken);
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, int tenantId, int? excludeContactId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Contacts
                .Where(c => c.PhoneNumber == phoneNumber && c.TenantId == tenantId && !c.IsDeleted);

            if (excludeContactId.HasValue)
            {
                query = query.Where(c => c.Id != excludeContactId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<PaginatedList<Contact>> GetPaginatedAsync(int tenantId, int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Contacts
                .Where(c => c.TenantId == tenantId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedOn)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(searchTerm) ||
                    c.PhoneNumber.Contains(searchTerm) ||
                    (c.Email != null && c.Email.Contains(searchTerm)) ||
                    (c.Company != null && c.Company.Contains(searchTerm)) ||
                    (c.Title != null && c.Title.Contains(searchTerm)));
            }

            var count = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<Contact>(items, count, pageNumber, pageSize);
        }
    }
}
