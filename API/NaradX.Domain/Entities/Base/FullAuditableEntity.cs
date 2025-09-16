using NaradX.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Base
{
    public abstract class FullAuditableEntity<TKey> : BaseEntity, ISoftDeletableEntity, IActivatableEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? DeactivatedOn { get; set; }
        public string? DeactivatedBy { get; set; }
    }

    public abstract class FullAuditableEntity : FullAuditableEntity<int>
    {
    }
}
