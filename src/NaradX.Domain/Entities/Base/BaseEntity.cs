using NaradX.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Base
{
    public abstract class BaseEntity<TKey> : IAuditableEntity
    {
        [Key]
        public TKey Id { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }
    public abstract class BaseEntity : BaseEntity<int>
    {
        // EF Core will handle auto-increment for int
    }

    public abstract class BaseGuidEntity : BaseEntity<Guid>
    {
        public BaseGuidEntity() => Id = Guid.NewGuid();
    }
}
