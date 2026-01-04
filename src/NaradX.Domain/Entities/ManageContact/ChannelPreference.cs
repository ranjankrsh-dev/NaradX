using NaradX.Domain.Entities.Base;
using NaradX.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.ManageContact
{
    public class ChannelPreference : FullAuditableEntity
    {
        public ChannelType ChannelType { get; set; }
        public DateTime? LastMessaged { get; set; }
        public string? ChannelSpecificId { get; set; } // PSID for Facebook, etc.

        // Foreign key to Contact
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; } = null!;
    }
}
