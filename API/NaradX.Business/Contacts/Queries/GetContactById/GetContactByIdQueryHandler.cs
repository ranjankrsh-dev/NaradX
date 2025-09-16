using AutoMapper;
using MediatR;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Queries.GetContactById
{
    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;

        public GetContactByIdQueryHandler(
            IContactRepository contactRepository,
            ITenantService tenantService,
            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _tenantService = tenantService;
            _mapper = mapper;
        }

        public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            if (!_tenantService.TenantId.HasValue)
                throw new UnauthorizedAccessException("Tenant not specified");

            var contact = await _contactRepository.GetByIdAsync(
                request.Id, _tenantService.TenantId.Value, cancellationToken);

            return contact == null ? null : _mapper.Map<ContactDto>(contact);
        }
    }
}
