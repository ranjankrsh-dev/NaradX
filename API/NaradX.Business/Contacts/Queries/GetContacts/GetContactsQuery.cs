using MediatR;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Models.Common;
using NaradX.Shared.Models.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Queries.GetContacts
{
    public class GetContactsQuery : IRequest<PaginatedList<ContactDto>>
    {
        public ContactFilterParams ContactFilter { get; set; }
        public GetContactsQuery(ContactFilterParams ContactFilter)
        {
            this.ContactFilter = ContactFilter;
        }
    }
}
