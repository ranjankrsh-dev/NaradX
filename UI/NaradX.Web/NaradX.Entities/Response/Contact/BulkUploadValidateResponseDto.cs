using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Response.Contact
{
    public class BulkUploadValidateResponseDto
    {
        public string BatchId { get; set; }
        public int TotalRows { get; set; }
        public int ValidRowsCount { get; set; }
        public int InvalidRowsCount { get; set; }
        public List<InvalidRowDto> InvalidRows { get; set; } = new();
        public string Message { get; set; }
    }
}
