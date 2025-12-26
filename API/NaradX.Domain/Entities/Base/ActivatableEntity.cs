// <copyright file="ActivatableEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Interfaces;

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
