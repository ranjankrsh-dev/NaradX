using AutoMapper;
using NaradX.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Mappings
{
    public class PaginatedListConverter<TSource, TDestination>
    : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(
            PaginatedList<TSource> source,
            PaginatedList<TDestination> destination,
            ResolutionContext context)
        {
            var items = context.Mapper.Map<List<TDestination>>(source.Items);

            // Calculate pageSize from available properties
            int pageSize = source.TotalCount > 0
                ? (int)Math.Ceiling((double)source.TotalCount / source.TotalPages)
                : 0;

            return new PaginatedList<TDestination>(
                items,
                source.TotalCount,
                source.PageNumber,
                source.PageSize
            );
        }
    }
}
