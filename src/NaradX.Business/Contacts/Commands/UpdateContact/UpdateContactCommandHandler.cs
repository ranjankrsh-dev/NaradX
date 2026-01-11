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
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, int>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContactCommandHandler(
            IContactRepository contactRepository,
            IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);

            if (contact == null)
                return 0;

            contact.FirstName = request.FirstName;
            contact.MiddleName = request.MiddleName;
            contact.LastName = request.LastName;
            contact.CountryId = request.CountryId;
            contact.LanguageId = request.LanguageId;
            contact.ChannelPreference = request.ChannelPreference;
            contact.ContactSource = request.ContactSource;
            contact.Company = request.Company;
            contact.JobTitle = request.JobTitle;
            contact.UpdatedOn = DateTime.UtcNow;

            _contactRepository.Update(contact);
            var resp = await _unitOfWork.SaveChangesAsync(cancellationToken);

            return resp;
        }

        private string ExtractCountryCode(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+") && phoneNumber.Length > 1)
                return phoneNumber.Substring(1, 2);
            return "1";
        }
    }
}
