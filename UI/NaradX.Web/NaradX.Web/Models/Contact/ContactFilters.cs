using NaradX.Entities.Common;

namespace NaradX.Web.Models.Contact
{
    public class ContactFilters : FilterParams
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
    }
}
