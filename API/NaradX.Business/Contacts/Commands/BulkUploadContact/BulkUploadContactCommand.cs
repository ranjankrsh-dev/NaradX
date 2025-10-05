using MediatR;
using NaradX.Shared.Dto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Commands.BulkUploadContact
{
    public class BulkUploadContactCommand : IRequest<ResponseDto>
    {
        public string BatchId { get; set; } = null!;
    }
}
