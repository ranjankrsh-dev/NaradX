using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Common
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }=null!;
        public string Code { get; set; }=null!;
        public string PhoneCode { get; set; }=null!;
        public string CurrencyCode { get; set; }=null!;
        public string CurrencySymbol { get; set; } = null!;
        public string Timezone { get; set; }=null!;
        public ICollection<Language> Languages { get; set; } = new List<Language>();
    }
}
