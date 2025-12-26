// <copyright file="ChangePasswordResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.ChangePassword
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ChangePasswordResponse
    {
        public string Message { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }
    }
}
