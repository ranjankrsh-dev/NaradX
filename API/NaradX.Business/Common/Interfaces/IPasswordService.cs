using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Interfaces
{
    public interface IPasswordService
    {
        (byte[] Hash, byte[] Salt) CreateHash(string password);
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}
