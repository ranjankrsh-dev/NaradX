using MediatR;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Commands.DeleteContact
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantService _tenantService;

        public DeleteContactCommandHandler(
            IContactRepository contactRepository,
            IUnitOfWork unitOfWork,
            ITenantService tenantService)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
            _tenantService = tenantService;
        }

        public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            if (!_tenantService.TenantId.HasValue)
                throw new UnauthorizedAccessException("Tenant not specified");

            var contact = await _contactRepository.GetByIdAsync(
                request.Id, _tenantService.TenantId.Value, cancellationToken);

            //if (contact == null)
            //    throw new NotFoundException("Contact not found");

            // Soft delete
            contact.IsDeleted = true;
            contact.DeletedOn = DateTime.UtcNow;

            _contactRepository.Update(contact);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
