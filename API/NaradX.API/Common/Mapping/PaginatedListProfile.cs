// <copyright file="PaginatedListProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Common.Mapping;

using AutoMapper;
using NaradX.Business.Common.Mappings;
using NaradX.Domain.Common;

public class PaginatedListProfile : Profile
{
    public PaginatedListProfile()
    {
        // Generic mapping for any PaginatedList
        this.CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
            .ConvertUsing(typeof(PaginatedListConverter<,>));
    }
}
