using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Common
{
    public class Language:BaseEntity
    {
        public int CountryId { get; set; }                   // <-- foreign key
        public Country Country { get; set; } = null!;        // <-- navigation
        public string Culture { get; set; } = null!;
        public string Name { get; set; }=null!; //Hindi
        public string LocalName { get; set; } = null!; // यूज़र
        public bool IsDefault { get; set; }
        public string? Description { get; set; }
    }
}
