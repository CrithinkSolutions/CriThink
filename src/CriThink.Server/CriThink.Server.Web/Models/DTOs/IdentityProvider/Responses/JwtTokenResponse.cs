using System;
using Newtonsoft.Json;
// ReSharper disable CheckNamespace

namespace CriThink.Web.Models.DTOs.IdentityProvider
{
    public class JwtTokenResponse
    {
        public JwtTokenResponse(string token, DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
        }

        [JsonProperty("token")]
        public string Token { get; }

        [JsonProperty("expirationDate")]
        public DateTime ExpirationDate { get; }
    }
}
