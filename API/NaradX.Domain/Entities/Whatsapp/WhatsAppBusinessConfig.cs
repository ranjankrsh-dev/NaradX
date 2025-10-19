using NaradX.Domain.Entities.Base;
using NaradX.Domain.Entities.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Whatsapp
{
    public class WhatsAppBusinessConfig : BaseEntity
    {
        public int TenantId { get; set; }
        public virtual Tenant Tenant { get; set; } = null!;
        public string AccessToken { get; set; } = string.Empty;
        public string PhoneNumberId { get; set; } = string.Empty; // Graph phone number id
        public string WabaId { get; set; } = string.Empty; // WhatsApp Business Account ID
    }
}
