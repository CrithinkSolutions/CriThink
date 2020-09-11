using System;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace

namespace CriThink.Common.Endpoints.DTOs.IdentityProvider
{
    public class JwtTokenResponse
    {
        public JwtTokenResponse(string token, DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
        }

        [JsonPropertyName("token")]
        public string Token { get; }

        [JsonPropertyName("expirationDate")]
        public DateTime ExpirationDate { get; }
    }
}
