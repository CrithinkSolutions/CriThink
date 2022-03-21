using System;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Models.Identity
{
    /// <summary>
    /// Represents the user credentials. Fields should have
    /// a private setter, but net core 3.1 does not allow it. 
    /// </summary>
    public class UserAccess
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserAccess() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="response">DTO</param>
        public UserAccess(UserLoginResponse response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            JwtToken = response.JwtToken;
            RefreshToken = response.RefreshToken;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jwtResponse">Access token</param>
        /// <param name="refreshToken">Refresh token</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserAccess(
           JwtTokenResponse jwtResponse,
           string refreshToken)
        {
            if (jwtResponse is null)
                throw new ArgumentNullException(nameof(jwtResponse));

            JwtToken = jwtResponse;
            RefreshToken = refreshToken;
        }

        [JsonInclude]
        public JwtTokenResponse JwtToken { get; internal set; }

        [JsonInclude]
        public string RefreshToken { get; internal set; }

        /// <summary>
        /// Updates the current JWT and refresh tokens
        /// </summary>
        /// <param name="newerTokens">The new JWT and refresh tokens</param>
        public void UpdateJwtTokens(UserRefreshTokenResponse newerTokens)
        {
            if (newerTokens is null)
                throw new ArgumentNullException(nameof(newerTokens));

            JwtToken = newerTokens.JwtToken;
            RefreshToken = newerTokens.RefreshToken;
        }
    }
}