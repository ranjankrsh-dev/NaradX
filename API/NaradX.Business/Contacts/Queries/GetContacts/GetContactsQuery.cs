using MediatR;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Queries.GetContacts
{
    public class GetContactsQuery : IRequest<PaginatedList<ContactDto>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
    }
}
