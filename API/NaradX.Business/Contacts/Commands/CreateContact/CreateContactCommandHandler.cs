using MediatR;
using NaradX.Business.Common.Interfaces;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Enums;
using NaradX.Domain.Repositories.Interfaces;

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
            var phoneExists = await _contactRepository.PhoneNumberExistsAsync(
                request.PhoneNumber, request.TenantId);

            if (phoneExists)
            {
                throw new InvalidOperationException("A contact with the same phone number already exists.");
            }

            var contact = new Contact
            {
                TenantId = request.TenantId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                ContactSource = request.ContactSource,
                CountryId = request.CountryId,
                LanguageId = request.LanguageId,
                Email = request.Email,
                Company = request.Company,
                Title = request.JobTitle,
                ImportSource = ImportSource.Manual
            };

            await _contactRepository.AddAsync(contact, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return contact.Id;
        }
    }

}
