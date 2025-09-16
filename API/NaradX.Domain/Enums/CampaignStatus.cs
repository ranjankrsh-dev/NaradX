using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum CampaignStatus
    {
        /// <summary>
        /// Campaign is being created
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Campaign is scheduled for future sending
        /// </summary>
        Scheduled = 2,

        /// <summary>
        /// Campaign is currently being processed
        /// </summary>
        Processing = 3,

        /// <summary>
        /// Campaign is actively sending messages
        /// </summary>
        Active = 4,

        /// <summary>
        /// Campaign completed successfully
        /// </summary>
        Completed = 5,

        /// <summary>
        /// Campaign was paused by user
        /// </summary>
        Paused = 6,

        /// <summary>
        /// Campaign failed during processing
        /// </summary>
        Failed = 7,

        /// <summary>
        /// Campaign was cancelled by user
        /// </summary>
        Cancelled = 8
    }
}
