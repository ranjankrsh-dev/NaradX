using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Models.Common;
using NaradX.Shared.Models.Contact;
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
                .Include(c => c.Country)
                .Include(c => c.Language)
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
                    (c.JobTitle != null && c.JobTitle.Contains(searchTerm)));
            }

            var count = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<Contact>(items, count, pageNumber, pageSize);
        }

        public async Task<PaginatedList<Contact>> GetContactsByFiltersAsync(ContactFilterParams filterParams, CancellationToken cancellationToken = default)
        {
            var query = _context.Contacts
            .Include(c => c.Country)
            .Include(c => c.Language)
            .Where(c => c.TenantId == filterParams.TenantId && !c.IsDeleted);

            // Apply search
            if (!string.IsNullOrEmpty(filterParams.SearchTerm))
            {
                query = ApplyContactSearch(query, filterParams.SearchTerm);
            }

            // Apply additional filters
            if (!string.IsNullOrEmpty(filterParams.Name))
            {
                query = query.Where(c =>
                    (c.FirstName + " " + c.LastName).Contains(filterParams.Name));
            }

            if (!string.IsNullOrEmpty(filterParams.Phone))
            {
                query = query.Where(c => c.PhoneNumber.Contains(filterParams.Phone));
            }

            if (!string.IsNullOrEmpty(filterParams.Status))
            {
                if (filterParams.Status.ToLower() == "enabled")
                    query = query.Where(c => c.Email != null);
                else if (filterParams.Status.ToLower() == "disabled")
                    query = query.Where(c => c.Email == null);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(filterParams.SortColumn))
            {
                query = ApplyContactSorting(query, filterParams.SortColumn, filterParams.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedOn);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var items = await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<Contact>(items, totalCount, filterParams.PageNumber, filterParams.PageSize);
        }

        public IQueryable<Contact> GetQueryable()
        {
            return _context.Contacts.AsQueryable();
        }

        private IQueryable<Contact> ApplyContactSearch(IQueryable<Contact> query, string searchTerm)
        {
            return query.Where(c =>
                c.FirstName.Contains(searchTerm) ||
                c.LastName.Contains(searchTerm) ||
                c.PhoneNumber.Contains(searchTerm) ||
                (c.Email != null && c.Email.Contains(searchTerm)) ||
                (c.Company != null && c.Company.Contains(searchTerm)) ||
                (c.JobTitle != null && c.JobTitle.Contains(searchTerm)));
        }

        private IQueryable<Contact> ApplyContactSorting(IQueryable<Contact> query, string sortColumn, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortColumn))
                return query;

            if (sortDirection?.ToLower() == "desc")
            {
                return query.OrderByDescending(e => EF.Property<object>(e, sortColumn));
            }
            else
            {
                return query.OrderBy(e => EF.Property<object>(e, sortColumn));
            }
        }

        public async Task<int> BulkContactSaveInDatabase(List<ContactDto> validContacts, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var contacts = validContacts.Select(c => new Contact
            {
                TenantId = 1,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MiddleName = c.MiddleName,
                CountryId = c.CountryId,
                LanguageId = c.LanguageId,
                PhoneNumber = c.PhoneNumber,
                ContactSource = c.ContactSource,
                ChannelPreference = c.ChannelPreference,
                Email = c.Email,
                Company = c.Company,
                JobTitle = c.JobTitle,
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            }).ToList();

            await _context.Contacts.AddRangeAsync(contacts, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
    }
}
