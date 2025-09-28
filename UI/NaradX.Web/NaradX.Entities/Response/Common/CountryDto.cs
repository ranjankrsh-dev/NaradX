using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Response.Common
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string Timezone { get; set; }
        public List<LanguageDto> Languages { get; set; } = new();

        public CountryDto(int id, string name, string code, string phoneCode, string currencyCode, string currencySymbol, string timezone)
        {
            this.Id = id;
            this.Name = name;
            this.Code = code;
            this.PhoneCode = phoneCode;
            this.CurrencyCode = currencyCode;
            this.CurrencySymbol = currencySymbol;
            this.Timezone = timezone;
        }
    }
}
