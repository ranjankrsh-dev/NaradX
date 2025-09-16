using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum MessageStatus
    {
        /// <summary>
        /// Message is created but not yet sent
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Message is queued for sending
        /// </summary>
        Queued = 2,

        /// <summary>
        /// Message is being processed
        /// </summary>
        Processing = 3,

        /// <summary>
        /// Message successfully sent to WhatsApp API
        /// </summary>
        Sent = 4,

        /// <summary>
        /// Message delivered to recipient's device
        /// </summary>
        Delivered = 5,

        /// <summary>
        /// Message was read by recipient
        /// </summary>
        Read = 6,

        /// <summary>
        /// Message failed to send
        /// </summary>
        Failed = 7,

        /// <summary>
        /// Message sending was cancelled
        /// </summary>
        Cancelled = 8
    }
}
