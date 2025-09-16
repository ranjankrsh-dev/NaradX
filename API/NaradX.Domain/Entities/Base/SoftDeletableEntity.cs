using NaradX.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Base
{
    public abstract class SoftDeletableEntity<TKey> : BaseEntity<TKey>, ISoftDeletableEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }

    public abstract class SoftDeletableEntity : SoftDeletableEntity<int>
    {
    }

    public abstract class SoftDeletableGuidEntity : SoftDeletableEntity<Guid>
    {
        public SoftDeletableGuidEntity() => Id = Guid.NewGuid();
    }
}
