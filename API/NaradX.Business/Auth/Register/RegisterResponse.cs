// <copyright file="RegisterResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.Register
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RegisterResponse
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
