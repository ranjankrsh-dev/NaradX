using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Common
{
    public abstract class FilterParams
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public FilterParams()
        {
            SearchTerm = string.Empty;
            SortColumn = string.Empty;
            SortDirection = "asc";
        }
    }
}
