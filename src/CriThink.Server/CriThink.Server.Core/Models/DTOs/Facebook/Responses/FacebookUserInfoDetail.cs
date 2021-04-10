using System.Text.Json.Serialization;
#pragma warning disable CA1056 // URI-like properties should not be strings

// ReSharper disable once CheckNamespace
namespace CriThink.Server.Core.Models.DTOs.Facebook
{
    public class FacebookUserInfoDetail
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("picture")]
        public Picture Picture { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Picture
    {
        [JsonPropertyName("data")]
        public UserInfoData Data { get; set; }
    }

    public class UserInfoData
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }
}
