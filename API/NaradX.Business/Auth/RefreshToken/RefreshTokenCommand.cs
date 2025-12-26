// <copyright file="RefreshTokenCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Auth.RefreshToken
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MediatR;
    using NaradX.Business.Auth.Login;

    public class RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;

        public string IpAddress { get; set; } = string.Empty;
    }
}
