using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.Roles.GetUserRoles
{
    public class GetUserRolesResponse
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsSystemRole { get; set; }
    }
}
