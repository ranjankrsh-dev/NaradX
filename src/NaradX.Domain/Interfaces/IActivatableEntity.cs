using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Interfaces
{
    public interface IActivatableEntity
    {
        bool IsActive { get; set; }
        DateTime? DeactivatedOn { get; set; }
        string? DeactivatedBy { get; set; }
    }
}
