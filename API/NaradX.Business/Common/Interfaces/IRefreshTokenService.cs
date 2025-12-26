// <copyright file="IRefreshTokenService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Auth;

    public interface IRefreshTokenService
    {
        Task<RefreshToken> GenerateRefreshTokenAsync(int userId, string ipAddress);

        Task<RefreshToken?> GetRefreshTokenAsync(string token);

        Task RevokeRefreshTokenAsync(string token, string ipAddress, string? replacedByToken = null);

        Task RevokeAllRefreshTokensForUserAsync(int userId, string ipAddress);

        Task<bool> IsRefreshTokenValidAsync(string token);
    }
}
