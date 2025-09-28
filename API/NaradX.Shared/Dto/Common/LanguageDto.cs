using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Dto.Common
{
    public class LanguageDto
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Culture { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public bool IsDefault { get; set; }
        public string? Description { get; set; }

        public LanguageDto(int id, int countryId, string culture, string name, string localName, bool isDefault, string? description)
        {
            Id = id;
            CountryId = countryId;
            Culture = culture;
            Name = name;
            LocalName = localName;
            IsDefault = isDefault;
            Description = description;
        }
    }
}
