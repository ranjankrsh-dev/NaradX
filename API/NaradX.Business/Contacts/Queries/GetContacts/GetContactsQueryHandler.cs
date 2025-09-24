using AutoMapper;
using MediatR;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Models.Common;
using NaradX.Shared.Models.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NaradX.Business.Contacts.Queries.GetContacts
{
    public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, PaginatedList<ContactDto>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;

        public GetContactsQueryHandler(
            IContactRepository contactRepository,
            ITenantService tenantService,
            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _tenantService = tenantService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ContactDto>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            var filterParams = new ContactFilterParams
            {
                TenantId = request.TenantId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SearchTerm = request.SearchTerm,
                Name = request.Name,
                Phone = request.Phone,
                Status = request.Status,
                SortColumn = request.SortColumn,
                SortDirection = request.SortDirection
            };

            var contacts = await _contactRepository.GetContactsByFiltersAsync(filterParams, cancellationToken);

            //var contacts = await _contactRepository.GetPaginatedAsync(
            //    request.TenantId,
            //    request.PageNumber,
            //    request.PageSize,
            //    request.SearchTerm,
            //    cancellationToken);

            return _mapper.Map<PaginatedList<ContactDto>>(contacts);
        }
    }
}
