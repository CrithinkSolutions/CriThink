using Newtonsoft.Json;
// ReSharper disable CheckNamespace

namespace CriThink.Web.Models.DTOs.IdentityProvider
{
    public class UserSignUpResponse
    {
        [JsonProperty("confirmationCode")]
        public string ConfirmationCode { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }
    }
}
