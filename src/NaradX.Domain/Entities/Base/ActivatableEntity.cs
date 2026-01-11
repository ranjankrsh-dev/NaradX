using NaradX.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Base
{
    public abstract class ActivatableEntity<TKey> : BaseEntity<TKey>, IActivatableEntity
    {
        public bool IsActive { get; set; } = true;
        public DateTime? DeactivatedOn { get; set; }
        public string? DeactivatedBy { get; set; }
    }

    public abstract class ActivatableEntity : ActivatableEntity<int>
    {
    }

    public abstract class ActivatableGuidEntity : ActivatableEntity<Guid>
    {
        public ActivatableGuidEntity() => Id = Guid.NewGuid();
    }
}
