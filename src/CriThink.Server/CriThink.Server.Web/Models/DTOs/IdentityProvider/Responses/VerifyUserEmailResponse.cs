using Newtonsoft.Json;
// ReSharper disable CheckNamespace

namespace CriThink.Web.Models.DTOs.IdentityProvider
{
    public class VerifyUserEmailResponse
    {
        [JsonProperty("jwtToken")]
        public JwtTokenResponse JwtToken { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }
    }
}
