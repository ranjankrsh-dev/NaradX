// <copyright file="IContactRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Repositories.Interfaces;

using NaradX.Domain.Common;
using NaradX.Domain.Entities.ManageContact;

/// <summary>
/// Repository interface for managing Contact entities.
/// </summary>
public interface IContactRepository : IRepository<Contact>
{
    /// <summary>
    /// Gets a contact by its identifier and tenant ID.
    /// </summary>
    /// <param name="id">The contact ID.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The contact if found; otherwise null.</returns>
    Task<Contact?> GetByIdAsync(int id, int tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a contact by its phone number and tenant ID.
    /// </summary>
    /// <param name="phoneNumber">The phone number.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The contact if found; otherwise null.</returns>
    Task<Contact?> GetByPhoneNumberAsync(string phoneNumber, int tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a phone number exists for a tenant.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check.</param>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="excludeContactId">Optional contact ID to exclude from the check.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the phone number exists; otherwise false.</returns>
    Task<bool> PhoneNumberExistsAsync(string phoneNumber, int tenantId, int? excludeContactId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated list of contacts for a tenant.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="searchTerm">Optional search term to filter contacts.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of contacts.</returns>
    Task<PaginatedList<Contact>> GetPaginatedAsync(int tenantId, int pageNumber, int pageSize, string? searchTerm = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets contacts filtered by the specified filter parameters.
    /// </summary>
    /// <param name="filterParams">The filter parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of filtered contacts.</returns>
    Task<PaginatedList<Contact>> GetContactsByFiltersAsync(ContactFilterParams filterParams, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a queryable collection of contacts.
    /// </summary>
    /// <returns>An IQueryable collection of contacts.</returns>
    IQueryable<Contact> GetQueryable();

    /// <summary>
    /// Bulk saves valid contacts to the database.
    /// </summary>
    /// <param name="validContacts">The list of valid contacts to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of contacts saved.</returns>
    Task<int> BulkContactSaveInDatabaseAsync(List<Contact> validContacts, CancellationToken cancellationToken = default);
}
