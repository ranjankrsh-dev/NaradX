using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Roles.AssignRole
{
    public class AssignRoleCommand : IRequest<AssignRoleResponse>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string AssignedBy { get; set; } = string.Empty;
    }
}
