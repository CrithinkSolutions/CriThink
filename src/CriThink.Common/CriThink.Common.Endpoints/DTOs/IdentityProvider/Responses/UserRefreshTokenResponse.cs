﻿using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class UserRefreshTokenResponse
    {
        [JsonPropertyName("token")]
        public JwtTokenResponse JwtToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
