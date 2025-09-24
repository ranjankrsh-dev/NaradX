using Microsoft.EntityFrameworkCore;
using NaradX.Domain.Entities.Base;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Models.Common;
using System.Linq;
using System.Linq.Expressions;

namespace NaradX.API.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<PaginatedList<TEntity>> GetPaginatedAsync<TEntity>(
            this IRepository<TEntity> repository,
            FilterParams filterParams,
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
            where TEntity : BaseEntity<int>
        {
            // Get base query
            var query = repository.GetQueryable();

            // Apply predicate if provided
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            // Apply search if search term is provided
            if (!string.IsNullOrEmpty(filterParams.SearchTerm))
            {
                query = ApplySearch(query, filterParams.SearchTerm);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(filterParams.SortColumn))
            {
                query = ApplySorting(query, filterParams.SortColumn, filterParams.SortDirection);
            }
            else
            {
                // Default sorting by ID
                query = query.OrderByDescending(e => e.Id);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var items = await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<TEntity>(items, totalCount, filterParams.PageNumber, filterParams.PageSize);
        }

        private static IQueryable<TEntity> ApplySearch<TEntity>(IQueryable<TEntity> query, string searchTerm)
            where TEntity : BaseEntity<int>
        {
            // This is a generic search implementation
            // For specific entity types, you might want to override this
            return query.Where(e =>
                EF.Functions.Like(e.Id.ToString(), $"%{searchTerm}%"));
        }

        private static IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> query, string sortColumn, string sortDirection)
            where TEntity : BaseEntity<int>
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

        // Helper method to get queryable from repository (you might need to implement this in your repository)
        private static IQueryable<TEntity> GetQueryable<TEntity>(this IRepository<TEntity> repository)
            where TEntity : BaseEntity<int>
        {
            // This is a placeholder - you'll need to implement this method in your actual repository
            // or use a different approach to get the IQueryable
            throw new NotImplementedException("You need to implement GetQueryable in your repository");
        }
    }
}
