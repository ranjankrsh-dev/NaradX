using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Commands.UpdateContact
{
    public class UpdateContactCommand : IRequest<int>
    {
        public int TenantId { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public int CountryId { get; set; }
        public int LanguageId { get; set; }
        public string ContactSource { get; set; } = null!;
        public string ChannelPreference { get; set; } = null!;
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
    }
}
