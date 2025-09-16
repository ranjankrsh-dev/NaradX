using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Request
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string Email { get; set; }
    }
}
