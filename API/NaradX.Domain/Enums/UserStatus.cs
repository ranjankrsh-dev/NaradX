using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum UserStatus
    {
        /// <summary>
        /// User is active and can access the system
        /// </summary>
        Active = 1,

        /// <summary>
        /// User account is created but not yet activated
        /// </summary>
        Inactive = 2,

        /// <summary>
        /// User account is temporarily suspended
        /// </summary>
        Suspended = 3,

        /// <summary>
        /// User account is locked due to multiple failed login attempts
        /// </summary>
        Locked = 4,

        /// <summary>
        /// User account is deactivated by admin
        /// </summary>
        Deactivated = 5
    }
}
