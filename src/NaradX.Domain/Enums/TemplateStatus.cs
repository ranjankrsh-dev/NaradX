using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum TemplateStatus
    {
        /// <summary>
        /// Template is being created
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Template submitted for WhatsApp approval
        /// </summary>
        PendingApproval = 2,

        /// <summary>
        /// Template approved by WhatsApp
        /// </summary>
        Approved = 3,

        /// <summary>
        /// Template rejected by WhatsApp
        /// </summary>
        Rejected = 4,

        /// <summary>
        /// Template is disabled
        /// </summary>
        Disabled = 5,

        /// <summary>
        /// Template is active and can be used
        /// </summary>
        Active = 6
    }
}
