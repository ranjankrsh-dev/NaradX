using AutoMapper;
using NaradX.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Mappings
{
    public class PaginatedListProfile : Profile
    {
        public PaginatedListProfile()
        {
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>));
        }
    }
}
