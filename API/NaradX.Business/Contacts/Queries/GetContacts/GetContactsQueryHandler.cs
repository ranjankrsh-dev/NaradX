// <copyright file="GetContactsQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Queries.GetContacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using NaradX.Business.Common.Interfaces;
    using NaradX.Domain.Repositories.Interfaces;
    using NaradX.Business.Dtos.Contact;
    using NaradX.Domain.Common;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var contacts = await _contactRepository.GetContactsByFiltersAsync(request.ContactFilter, cancellationToken);

            return _mapper.Map<PaginatedList<ContactDto>>(contacts);
        }
    }
}
