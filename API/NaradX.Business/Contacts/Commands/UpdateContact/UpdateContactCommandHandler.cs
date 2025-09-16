using MediatR;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Commands.UpdateContact
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantService _tenantService;

        public UpdateContactCommandHandler(
            IContactRepository contactRepository,
            IUnitOfWork unitOfWork,
            ITenantService tenantService)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
            _tenantService = tenantService;
        }

        public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            if (!_tenantService.TenantId.HasValue)
                throw new UnauthorizedAccessException("Tenant not specified");

            var contact = await _contactRepository.GetByIdAsync(
                request.Id, _tenantService.TenantId.Value, cancellationToken);

            //if (contact == null)
            //    throw new NotFoundException("Contact not found");

            // Check for duplicate excluding current contact
            var phoneExists = await _contactRepository.PhoneNumberExistsAsync(
                request.PhoneNumber, _tenantService.TenantId.Value, request.Id, cancellationToken);

            //if (phoneExists)
            //    throw new ConflictException("Another contact with this phone number already exists");

            contact.FirstName = request.ContactName;
            contact.PhoneNumber = request.PhoneNumber;
            contact.CountryCode = ExtractCountryCode(request.PhoneNumber);
            contact.Email = request.Email;
            contact.Company = request.Company;
            contact.Title = request.Title;
            contact.LanguagePreference = request.LanguagePreference;
            contact.Timezone = request.Timezone;
            contact.UpdatedOn = DateTime.UtcNow;

            _contactRepository.Update(contact);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private string ExtractCountryCode(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+") && phoneNumber.Length > 1)
                return phoneNumber.Substring(1, 2);
            return "1";
        }
    }
}
