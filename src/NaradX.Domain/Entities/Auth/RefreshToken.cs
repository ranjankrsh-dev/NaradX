using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Auth
{
    public class RefreshToken : BaseEntity<int>
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        public DateTime Expires { get; set; }
        public DateTime? Revoked { get; set; }

        [Required]
        public string CreatedByIp { get; set; } = string.Empty;

        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsRevoked && !IsExpired;

        // Foreign key
        public int UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
