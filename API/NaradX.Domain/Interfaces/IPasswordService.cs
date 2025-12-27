// <copyright file="IPasswordService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPasswordService
    {
        (byte[] Hash, byte[] Salt) CreateHash(string password);

        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}
