using CriThink.Common.Endpoints.Converters;
using System;
using System.Text.Json.Serialization;

namespace CriThink.Server.Web.Models.DTOs.Google
{
    public class TokenInfo
    {
        [JsonPropertyName("aud")]
        public string ApplicationId { get; set; }

        [JsonPropertyName("sub")]
        public string UserId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("email_verified")]
        [JsonConverter(typeof(StringToBoolConverter))]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("picture")]
        public string Picture { get; set; }

        [JsonPropertyName("given_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("family_name")]
        public string LastName { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("iat")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime IssuedAt { get; set; }

        [JsonPropertyName("exp")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime ExpiresAtUtc { get; set; }

        [JsonPropertyName("alg")]
        public string EncryptionAlgorithm { get; set; }

        [JsonPropertyName("typ")]
        public string CertificateType { get; set; }

        [JsonPropertyName("iss")]
        public string Issuer { get; set; }

        [JsonPropertyName("azp")]
        public string Azp { get; set; }

        [JsonPropertyName("kid")]
        public string Kid { get; set; }
    }

}
