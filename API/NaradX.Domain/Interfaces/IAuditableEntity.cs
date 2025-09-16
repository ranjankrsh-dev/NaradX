﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedOn { get; set; }
        string? CreatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        string? UpdatedBy { get; set; }
    }
}
