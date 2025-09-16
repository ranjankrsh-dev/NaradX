using AutoMapper;
using MediatR;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Common.Model;
using NaradX.Shared.Dto.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //if (!_tenantService.TenantId.HasValue)
            //    throw new UnauthorizedAccessException("Tenant not specified");

            var contacts = await _contactRepository.GetPaginatedAsync(
                1,//_tenantService.TenantId.Value
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                cancellationToken);

            return _mapper.Map<PaginatedList<ContactDto>>(contacts);
        }
    }
}
