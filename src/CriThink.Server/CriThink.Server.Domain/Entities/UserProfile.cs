using System;

namespace CriThink.Server.Domain.Entities
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

        public virtual User User { get; private set; }

        #region Create

        public static UserProfile Create()
        {
            return new UserProfile(DateTime.UtcNow);
        }

        public static UserProfile CreateSeed(
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

        internal void UpdateGivenName(string givenName)
        {
            GivenName = givenName;
        }

        internal void UpdateGender(Gender? gender)
        {
            Gender = gender;
        }

        internal void UpdateTelegram(string telegram)
        {
            Telegram = telegram;
        }

        internal void UpdateTwitter(string twitter)
        {
            Twitter = twitter;
        }

        internal void UpdateUserAvatar(string path)
        {
            AvatarPath = path;
        }

        internal void UpdateFacebook(string facebook)
        {
            Facebook = facebook;
        }

        internal void UpdateBlog(string blog)
        {
            Blog = blog;
        }

        internal void UpdateDateOfBirth(DateTime? dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
        }

        internal void UpdateYoutube(string youtube)
        {
            Youtube = youtube;
        }

        internal void UpdateSnapchat(string snapchat)
        {
            Snapchat = snapchat;
        }

        internal void UpdateInstagram(string instagram)
        {
            Instagram = instagram;
        }

        internal void UpdateSkype(string skype)
        {
            Skype = skype;
        }

        internal void UpdateCountry(string country)
        {
            Country = country;
        }

        internal void UpdateDescription(string description)
        {
            Description = description;
        }

        internal void UpdateFamilyName(string familyName)
        {
            FamilyName = familyName;
        }
    }
}