// <copyright file="PaginatedListConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Common.Mapping
{
    using System.Collections.Generic;
    using AutoMapper;
    using NaradX.Domain.Common;

    public class PaginatedListConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        public PaginatedList<TDestination> Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination, ResolutionContext context)
        {
            var sourceItems = source.Items;
            var destinationItems = context.Mapper.Map<List<TDestination>>(sourceItems);

            return new PaginatedList<TDestination>(destinationItems, source.TotalCount, source.PageNumber, source.PageSize);
        }
    }
}
