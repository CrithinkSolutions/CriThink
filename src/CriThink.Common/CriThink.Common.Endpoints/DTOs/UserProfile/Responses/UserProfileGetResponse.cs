using System;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace CriThink.Common.Endpoints.DTOs.UserProfile
{
    public class UserProfileGetResponse
    {
        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("familyName")]
        public string FamilyName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("gender")]
        public GenderDto? Gender { get; set; }

        [JsonPropertyName("avatarPath")]
        public string AvatarPath { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("telegram")]
        public string Telegram { get; set; }

        [JsonPropertyName("skype")]
        public string Skype { get; set; }

        [JsonPropertyName("twitter")]
        public string Twitter { get; set; }

        [JsonPropertyName("instagram")]
        public string Instagram { get; set; }

        [JsonPropertyName("facebook")]
        public string Facebook { get; set; }

        [JsonPropertyName("snapchat")]
        public string Snapchat { get; set; }

        [JsonPropertyName("youtube")]
        public string Youtube { get; set; }

        [JsonPropertyName("blog")]
        public string Blog { get; set; }

        [JsonPropertyName("dob")]
        public DateTime? DateOfBirth { get; set; }

        [JsonPropertyName("registeredOn")]
        public string RegisteredOn { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
