using NaradX.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Models.Contact
{
    public class ContactFilterParams : FilterParams
    {
        public int TenantId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }

        public ContactFilterParams() : base()
        {
            Name = string.Empty;
            Phone = string.Empty;
            Status = string.Empty;
        }

        public ContactFilterParams(
            int tenantId,
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? name = null,
            string? phone = null,
            string? status = null,
            string? sortColumn = null,
            string? sortDirection = "asc")
            : base(pageNumber, pageSize, searchTerm, sortColumn, sortDirection)
        {
            TenantId = tenantId;
            Name = name ?? string.Empty;
            Phone = phone ?? string.Empty;
            Status = status ?? string.Empty;
        }
    }
}
