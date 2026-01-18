using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.ManageContact
{
    public class ContactTag:BaseEntity
    {
        // Composite primary key
        public int ContactId { get; set; }
        public int TagId { get; set; }

        // Navigation properties
        public virtual Contact Contact { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
