using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ChangePasswordResponse>
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
