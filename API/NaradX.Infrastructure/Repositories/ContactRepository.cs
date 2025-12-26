// <copyright file="ContactRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using NaradX.Business.Dtos.Contact;
    using NaradX.Domain.Common;
    using NaradX.Domain.Entities.ManageContact;
    using NaradX.Domain.Repositories.Interfaces;

    /// <summary>
    /// Repository for managing Contact entities with pagination, filtering, and search capabilities.
    /// </summary>
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ContactRepository(NaradXDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets a contact by its ID and tenant ID, excluding deleted contacts.
        /// </summary>
        /// <param name="id">The contact ID.</param>
        /// <param name="tenantId">The tenant ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The contact if found; otherwise, null.</returns>
        public async Task<Contact?> GetByIdAsync(int id, int tenantId, CancellationToken cancellationToken = default)
        {
            return await this._context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.TenantId == tenantId && !c.IsDeleted, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a contact by its phone number and tenant ID, excluding deleted contacts.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="tenantId">The tenant ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The contact if found; otherwise, null.</returns>
        public async Task<Contact?> GetByPhoneNumberAsync(string phoneNumber, int tenantId, CancellationToken cancellationToken = default)
        {
            return await this._context.Contacts
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.TenantId == tenantId && !c.IsDeleted, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Checks if a phone number exists for a specific tenant.
        /// </summary>
        /// <param name="phoneNumber">The phone number to check.</param>
        /// <param name="tenantId">The tenant ID.</param>
        /// <param name="excludeContactId">Optional contact ID to exclude from the check.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the phone number exists; otherwise, false.</returns>
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, int tenantId, int? excludeContactId = null, CancellationToken cancellationToken = default)
        {
            var query = this._context.Contacts
                .Where(c => c.PhoneNumber == phoneNumber && c.TenantId == tenantId && !c.IsDeleted);

            if (excludeContactId.HasValue)
            {
                query = query.Where(c => c.Id != excludeContactId.Value);
            }

            return await query.AnyAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a paginated list of contacts for a specific tenant with optional search term.
        /// </summary>
        /// <param name="tenantId">The tenant ID.</param>
        /// <param name="pageNumber">The page number (1-indexed).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="searchTerm">Optional search term to filter contacts.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paginated list of contacts.</returns>
        public async Task<PaginatedList<Contact>> GetPaginatedAsync(int tenantId, int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default)
        {
            var query = this._context.Contacts
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

            var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return new PaginatedList<Contact>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// Gets a paginated list of contacts filtered by the provided filter parameters.
        /// </summary>
        /// <param name="filterParams">The filter parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paginated list of filtered contacts.</returns>
        /// <exception cref="ArgumentNullException">Thrown when filterParams is null.</exception>
        public async Task<PaginatedList<Contact>> GetContactsByFiltersAsync(ContactFilterParams filterParams, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(filterParams);

            var query = this._context.Contacts
                .Include(c => c.Country)
                .Include(c => c.Language)
                .Where(c => c.TenantId == filterParams.TenantId && !c.IsDeleted);

            // Apply search
            if (!string.IsNullOrEmpty(filterParams.SearchTerm))
            {
                query = this.ApplyContactSearch(query, filterParams.SearchTerm);
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
                if (string.Equals(filterParams.Status, "enabled", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Email != null);
                }
                else if (string.Equals(filterParams.Status, "disabled", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Email == null);
                }
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(filterParams.SortColumn))
            {
                query = this.ApplyContactSorting(query, filterParams.SortColumn, filterParams.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(c => c.CreatedOn);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            // Apply pagination
            var items = await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return new PaginatedList<Contact>(items, totalCount, filterParams.PageNumber, filterParams.PageSize);
        }

        /// <summary>
        /// Gets an IQueryable sequence of contacts for advanced query operations.
        /// </summary>
        /// <returns>An IQueryable sequence of contacts.</returns>
        public IQueryable<Contact> GetQueryable()
        {
            return this._context.Contacts.AsQueryable();
        }

        /// <summary>
        /// Applies a search filter to a contact query.
        /// </summary>
        /// <param name="query">The contact query.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The filtered query.</returns>
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

        /// <summary>
        /// Applies sorting to a contact query.
        /// </summary>
        /// <param name="query">The contact query.</param>
        /// <param name="sortColumn">The column to sort by.</param>
        /// <param name="sortDirection">The sort direction (asc or desc).</param>
        /// <returns>The sorted query.</returns>
        private IQueryable<Contact> ApplyContactSorting(IQueryable<Contact> query, string sortColumn, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortColumn))
            {
                return query;
            }

            if (string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase))
            {
                return query.OrderByDescending(e => EF.Property<object>(e, sortColumn));
            }
            else
            {
                return query.OrderBy(e => EF.Property<object>(e, sortColumn));
            }
        }

        /// <summary>
        /// Bulk saves a list of contacts to the database with transaction support.
        /// </summary>
        /// <param name="validContacts">The list of contacts to save.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="ArgumentNullException">Thrown when validContacts is null.</exception>
        public async Task<int> BulkContactSaveInDatabaseAsync(List<Contact> validContacts, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(validContacts);

            using var transaction = await this._context.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            // Set TenantId and timestamps for each contact
            foreach (var contact in validContacts)
            {
                contact.TenantId = contact.TenantId > 0 ? contact.TenantId : 1;
                contact.IsActive = true;
            }

            await this._context.Contacts.AddRangeAsync(validContacts, cancellationToken).ConfigureAwait(false);
            var result = await this._context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return result;
        }
    }
}
