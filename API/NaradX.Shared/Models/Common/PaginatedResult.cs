using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Models.Common
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public int PageSize { get; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedResult(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }

        // Additional constructor for mapping between different PaginatedList types
        public PaginatedList<TResult> Map<TResult>(Func<T, TResult> mapFunction)
        {
            var mappedItems = Items.Select(mapFunction).ToList();
            return new PaginatedList<TResult>(mappedItems, TotalCount, PageNumber, PageSize);
        }
    }
}
