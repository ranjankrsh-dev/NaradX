using Microsoft.Extensions.Options;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Common.Models;
using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Common.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(
            IRepository<RefreshToken> refreshTokenRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId, string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateRandomToken(),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                CreatedByIp = ipAddress,
                UserId = userId
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _refreshTokenRepository.FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task RevokeRefreshTokenAsync(string token, string ipAddress, string? replacedByToken = null)
        {
            var refreshToken = await GetRefreshTokenAsync(token);
            if (refreshToken == null || refreshToken.IsRevoked)
                return;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = replacedByToken;

            _refreshTokenRepository.Update(refreshToken);
        }

        public async Task RevokeAllRefreshTokensForUserAsync(int userId, string ipAddress)
        {
            var refreshTokens = await _refreshTokenRepository.FindAsync(rt =>
                rt.UserId == userId && rt.IsActive);

            foreach (var token in refreshTokens)
            {
                token.Revoked = DateTime.UtcNow;
                token.RevokedByIp = ipAddress;
                _refreshTokenRepository.Update(token);
            }
        }

        public async Task<bool> IsRefreshTokenValidAsync(string token)
        {
            var refreshToken = await GetRefreshTokenAsync(token);
            return refreshToken != null && refreshToken.IsActive;
        }

        private string GenerateRandomToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
