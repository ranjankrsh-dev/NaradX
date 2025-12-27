// <copyright file="DeleteContactCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Commands.DeleteContact
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;
    using NaradX.Business.Interfaces;
    using NaradX.Domain.Interfaces;

    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, int>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContactCommandHandler(
            IContactRepository contactRepository,
            IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);

            if (contact == null)
                return 0;

            contact.IsDeleted = true;
            contact.DeletedOn = DateTime.UtcNow;

            _contactRepository.Update(contact);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
