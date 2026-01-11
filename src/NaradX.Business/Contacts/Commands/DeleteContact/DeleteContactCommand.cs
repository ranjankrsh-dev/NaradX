using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Commands.DeleteContact
{
    public class DeleteContactCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
