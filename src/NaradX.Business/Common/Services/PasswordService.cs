using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using NaradX.Business.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public (byte[] Hash, byte[] Salt) CreateHash(string password)
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            return (hash, salt);
        }

        public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            var hashToVerify = KeyDerivation.Pbkdf2(
                password: password,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize
            );

            return CryptographicOperations.FixedTimeEquals(hashToVerify, storedHash);
        }
    }
}
