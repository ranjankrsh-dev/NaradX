using AutoMapper;
using NaradX.Business.Common.Mappings;
using NaradX.Shared.Models.Common;

namespace NaradX.API.Common.Mapping
{
    public class PaginatedListProfile : Profile
    {
        public PaginatedListProfile()
        {
            // Generic mapping for any PaginatedList
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                .ConvertUsing(typeof(PaginatedListConverter<,>));
        }
    }
}
