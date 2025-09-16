using MediatR;
using NaradX.Shared.Common.Model;
using NaradX.Shared.Dto.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Queries.GetContacts
{
    public class GetContactsQuery : IRequest<PaginatedList<ContactDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? Tag { get; set; }
    }
}
