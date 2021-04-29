﻿using System.Security.Claims;
using System.Threading.Tasks;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Server.Core.Entities;

namespace CriThink.Server.Core.Interfaces
{
    public interface IJwtManager
    {
        /// <summary>
        /// Generates a JWT token for the given user
        /// </summary>
        /// <param name="user">User to associate the token</param>
        /// <returns>The JWT token with details</returns>
        Task<JwtTokenResponse> GenerateUserJwtTokenAsync(User user);

        /// <summary>
        /// Generate a random string to be used as refresh token by a specific user
        /// </summary>
        /// <returns>A random string</returns>
        string GenerateToken();

        /// <summary>
        /// Get claims principal from a JWT token. Token is
        /// validated before being read.
        /// </summary>
        /// <param name="token">The token to read</param>
        /// <returns>The claim principals</returns>
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
