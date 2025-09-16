using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Auth.ChangePassword
{
    public class ChangePasswordResponse
    {
        public string Message { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}
