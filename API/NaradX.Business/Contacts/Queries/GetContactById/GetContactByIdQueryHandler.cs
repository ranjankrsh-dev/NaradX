// <copyright file="GetContactByIdQueryHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Queries.GetContactById
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

    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public GetContactByIdQueryHandler(
            IContactRepository contactRepository,
            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(
                request.Id, cancellationToken);

            return _mapper.Map<ContactDto>(contact);
        }
    }
}
