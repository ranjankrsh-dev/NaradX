using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum RoleType
    {
        /// <summary>
        /// Full system administrator (across all tenants)
        /// </summary>
        SuperAdmin = 1,

        /// <summary>
        /// Administrator for a specific tenant
        /// </summary>
        TenantAdmin = 2,

        /// <summary>
        /// Regular user with basic permissions
        /// </summary>
        User = 3,

        /// <summary>
        /// User who can create campaigns but not manage users
        /// </summary>
        CampaignManager = 4,

        /// <summary>
        /// User who can only view reports
        /// </summary>
        Reporter = 5,

        /// <summary>
        /// Read-only access for auditors
        /// </summary>
        Auditor = 6
    }
}
