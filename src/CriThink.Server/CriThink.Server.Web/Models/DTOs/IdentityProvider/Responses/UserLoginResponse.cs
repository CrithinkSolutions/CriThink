using Newtonsoft.Json;
// ReSharper disable CheckNamespace

namespace CriThink.Web.Models.DTOs.IdentityProvider
{
    public class UserLoginResponse
    {
        [JsonProperty("token")]
        public JwtTokenResponse JwtToken { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}
