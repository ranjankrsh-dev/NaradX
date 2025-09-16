using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Roles.GetUserRoles
{
    public class GetUserRolesQuery : IRequest<GetUserRolesResponse>
    {
        public int UserId { get; set; }
    }
}
