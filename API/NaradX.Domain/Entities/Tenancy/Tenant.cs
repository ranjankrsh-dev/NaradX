using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Entities.Base;
using NaradX.Domain.Entities.ManageContact;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Tenancy
{
    public class Tenant : FullAuditableEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Domain { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? ContactEmail { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public DateTime SubscriptionStartDate { get; set; } = DateTime.UtcNow;
        public DateTime? SubscriptionEndDate { get; set; }

        public int MaxUsers { get; set; } = 5;
        public int MaxContacts { get; set; } = 1000;
        public int MaxMessagesPerMonth { get; set; } = 10000;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
