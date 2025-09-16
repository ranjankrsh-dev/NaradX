using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    /// <summary>
    /// When to use: For GDPR/compliance tracking of user consent for communications.
    /// Why needed: Legal requirement for messaging systems to track user consent status and prevent spam
    /// </summary>
    public enum OptInStatus
    {
        /// <summary>
        /// User explicitly consented to receive messages.
        /// </summary>
        OptedIn,
        /// <summary>
        /// User explicitly refused/unsubscribed
        /// </summary>
        OptedOut,
        /// <summary>
        /// Consent requested but not yet confirmed (double opt-in)
        /// </summary>
        Pending,
        /// <summary>
        /// Consent was valid but expired after time period
        /// </summary>
        Expired
    }
}
