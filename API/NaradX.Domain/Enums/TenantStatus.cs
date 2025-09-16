using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum TenantStatus
    {
        /// <summary>
        /// Tenant is active and can use the system
        /// </summary>
        Active = 1,

        /// <summary>
        /// Tenant is in trial period (e.g., 14 days free trial)
        /// </summary>
        Trial = 2,

        /// <summary>
        /// Tenant subscription has expired
        /// </summary>
        Expired = 3,

        /// <summary>
        /// Tenant account is suspended due to policy violation
        /// </summary>
        Suspended = 4,

        /// <summary>
        /// Tenant has cancelled subscription but data is retained
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// Tenant account is pending approval
        /// </summary>
        Pending = 6
    }
}
