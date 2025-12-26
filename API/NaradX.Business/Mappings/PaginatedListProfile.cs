// <copyright file="PaginatedListProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using NaradX.Domain.Common;

    public class PaginatedListProfile : Profile
    {
        public PaginatedListProfile()
        {
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>));
        }
    }
}
