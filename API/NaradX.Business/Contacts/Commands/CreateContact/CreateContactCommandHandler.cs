using MediatR;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Enums;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Commands.CreateContact
{
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, int>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantService _tenantService;

        public CreateContactCommandHandler(
            IContactRepository contactRepository,
            IUnitOfWork unitOfWork,
            ITenantService tenantService)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
            _tenantService = tenantService;
        }

        public async Task<int> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            //if (!_tenantService.TenantId.HasValue)
            //    throw new UnauthorizedAccessException("Tenant not specified");

            // Check for duplicate using repository
            var phoneExists = await _contactRepository.PhoneNumberExistsAsync(
                request.PhoneNumber, 1);

            //if (phoneExists)
            //    throw new ConflictException("Contact with this phone number already exists");

            var contact = new Contact
            {
                TenantId = 1,
                FirstName = request.ContactName,
                PhoneNumber = request.PhoneNumber,
                CountryCode = ExtractCountryCode(request.PhoneNumber),
                Email = request.Email,
                Company = request.Company,
                Title = request.Title,
                LanguagePreference = request.LanguagePreference,
                Timezone = request.Timezone,
                Source = ContactSource.Manual
            };

            await _contactRepository.AddAsync(contact, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return contact.Id;
        }

        private string ExtractCountryCode(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+") && phoneNumber.Length > 1)
                return phoneNumber.Substring(1, 2);
            return "1";
        }
    }

}
