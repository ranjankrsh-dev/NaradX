using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Roles.RemoveRole
{
    public class RemoveRoleCommand : IRequest<RemoveRoleResponse>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RemovedBy { get; set; } = string.Empty;
    }
}
