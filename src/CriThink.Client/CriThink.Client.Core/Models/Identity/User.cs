using System;
using System.Globalization;
using System.Text.Json.Serialization;
using CriThink.Common.Endpoints.DTOs.UserProfile;

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
        public User(UserProfileGetResponse response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));

            Id = response.UserId;
            Email = response.Email;
            Username = response.Username;
            GivenName = response.GivenName;
            FamilyName = response.FamilyName;
            Description = response.Description;
            Gender = response.Gender;
            Country = response.Country;
            Telegram = response.Telegram;
            Skype = response.Skype;
            Twitter = response.Twitter;
            Instagram = response.Instagram;
            Facebook = response.Facebook;
            Snapchat = response.Snapchat;
            Youtube = response.Youtube;
            Blog = response.Blog;
            DateOfBirth = response.DateOfBirth;
            RegisteredOn = DateTime.Parse(response.RegisteredOn, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            AvatarPath = GetAvatarPath(response.AvatarPath);
        }

        [JsonInclude]
        public Guid Id { get; internal set; }

        [JsonInclude]
        public string Email { get; internal set; }

        [JsonInclude]
        public string Username { get; internal set; }

        [JsonInclude]
        public string GivenName { get; internal set; }

        [JsonInclude]
        public string FamilyName { get; internal set; }

        [JsonInclude]
        public string Description { get; internal set; }

        [JsonInclude]
        public GenderDto? Gender { get; internal set; }

        [JsonInclude]
        public string AvatarPath { get; internal set; }

        [JsonInclude]
        public string Country { get; internal set; }

        [JsonInclude]
        public string Telegram { get; internal set; }

        [JsonInclude]
        public string Skype { get; internal set; }

        [JsonInclude]
        public string Twitter { get; internal set; }

        [JsonInclude]
        public string Instagram { get; internal set; }

        [JsonInclude]
        public string Facebook { get; internal set; }

        [JsonInclude]
        public string Snapchat { get; internal set; }

        [JsonInclude]
        public string Youtube { get; internal set; }

        [JsonInclude]
        public string Blog { get; internal set; }

        [JsonInclude]
        public DateTime? DateOfBirth { get; internal set; }

        [JsonInclude]
        public DateTime RegisteredOn { get; internal set; }

        private static string GetAvatarPath(string avatarPath) =>
            string.IsNullOrWhiteSpace(avatarPath) ? "ic_logo.svg" : avatarPath;
    }
}
