using MediatR;
using NaradX.Shared.Dto.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Queries.GetContactById
{
    public class GetContactByIdQuery : IRequest<ContactDto?>
    {
        public int Id { get; set; }
    }
}
