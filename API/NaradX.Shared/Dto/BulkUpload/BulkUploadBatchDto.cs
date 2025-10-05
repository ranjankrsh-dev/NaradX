using NaradX.Shared.Dto.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Dto.BulkUpload
{
    public class BulkUploadBatch
    {
        public string BatchId { get; set; }
        public List<ContactDto> ValidContacts { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
