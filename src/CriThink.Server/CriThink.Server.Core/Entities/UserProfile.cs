using System;
using System.ComponentModel.DataAnnotations;

namespace CriThink.Server.Core.Entities
{
    /// <summary>
    /// Database entity representing the user information
    /// </summary>
    public class UserProfile : Entity<Guid>
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        protected UserProfile()
        { }

        private UserProfile(DateTime registeredOn)
        {
            RegisteredOn = registeredOn;
        }

        private UserProfile(
            Guid id,
            DateTime registeredOn,
            DateTime dateOfBirth,
            string description,
            Guid userId)
        {
            Id = id;
            RegisteredOn = registeredOn;
            DateOfBirth = dateOfBirth;
            Description = description;
            UserId = userId;
        }
        public string GivenName { get; private set; }

        public string FamilyName { get; private set; }

        public string Description { get; private set; }

        public Gender? Gender { get; private set; }

        public string AvatarPath { get; private set; }

        public string Country { get; private set; }

        public string Telegram { get; private set; }

        public string Skype { get; private set; }

        public string Twitter { get; private set; }

        public string Instagram { get; private set; }

        public string Facebook { get; private set; }

        public string Snapchat { get; private set; }

        public string Youtube { get; private set; }

        public string Blog { get; private set; }

        public DateTime? DateOfBirth { get; private set; }

        public DateTime RegisteredOn { get; private set; }

        public Guid UserId { get; private set; }

        public User User { get; private set; }

        #region Create

        public static UserProfile Create()
        {
            return new UserProfile(DateTime.UtcNow);
        }

        public static UserProfile Create(
            Guid id,
            DateTime registeredOn,
            DateTime dateOfBirth,
            string description,
            Guid userId)
        {
            return new UserProfile(
                id,
                registeredOn,
                dateOfBirth,
                description,
                userId);
        }

        #endregion

        public void UpdateGivenName(string givenName)
        {
            GivenName = givenName;
        }

        public void UpdateGender(Gender? gender)
        {
            Gender = gender;
        }

        public void UpdateTelegram(string telegram)
        {
            Telegram = telegram;
        }

        public void UpdateTwitter(string twitter)
        {
            Twitter = twitter;
        }

        public void UpdateUserAvatar(string path)
        {
            AvatarPath = path;
        }

        public void UpdateFacebook(string facebook)
        {
            Facebook = facebook;
        }

        public void UpdateBlog(string blog)
        {
            Blog = blog;
        }

        public void UpdateDateOfBirth(DateTime? dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
        }

        public void UpdateYoutube(string youtube)
        {
            Youtube = youtube;
        }

        public void UpdateSnapchat(string snapchat)
        {
            Snapchat = snapchat;
        }

        public void UpdateInstagram(string instagram)
        {
            Instagram = instagram;
        }

        public void UpdateSkype(string skype)
        {
            Skype = skype;
        }

        public void UpdateCountry(string country)
        {
            Country = country;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateFamilyName(string familyName)
        {
            FamilyName = familyName;
        }
    }
}