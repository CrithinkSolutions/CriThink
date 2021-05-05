using System;
using System.Globalization;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;

namespace CriThink.Client.Core.Models.Identity
{
    /// <summary>
    /// Represents the logged user. Fields should have
    /// a private setter, but net core 3.1 does not allow it. 
    /// </summary>
    public class User
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public User() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="response">DTO</param>
        public User(UserLoginResponse response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            UserId = response.UserId;
            UserEmail = response.UserEmail;
            UserName = response.UserName;
            JwtToken = response.JwtToken;
            RefreshToken = response.RefreshToken;
            AvatarPath = GetAvatarPath(response.AvatarPath);
            RegisteredOn = DateTime.Parse(response.RegisteredOn, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            //TODO: add about description
        }

        [JsonInclude]
        public string UserId { get; internal set; }

        [JsonInclude]
        public string UserEmail { get; internal set; }

        [JsonInclude]
        public string UserName { get; internal set; }

        [JsonInclude]
        public string AvatarPath { get; internal set; }

        [JsonInclude]
        public JwtTokenResponse JwtToken { get; internal set; }

        [JsonInclude]
        public string RefreshToken { get; internal set; }

        [JsonInclude]
        public DateTime RegisteredOn { get; internal set; }

        [JsonInclude]
        public string AboutDescription { get; internal set; }

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

        private static string GetAvatarPath(string avatarPath) =>
            string.IsNullOrWhiteSpace(avatarPath) ? "ic_logo.svg" : avatarPath;
    }
}
