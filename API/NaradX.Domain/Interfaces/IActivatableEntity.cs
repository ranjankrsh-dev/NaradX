// <copyright file="IActivatableEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IActivatableEntity
    {
        bool IsActive { get; set; }

        DateTime? DeactivatedOn { get; set; }

        string? DeactivatedBy { get; set; }
    }
}
