using NaradX.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user, IEnumerable<UserRole> role);
        int? ValidateToken(string token);
    }
}
