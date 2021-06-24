using System.Collections.Generic;
using System.Linq;
using CriThink.Client.Core.Models.Identity;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Common
{
    public class UserProfileViewModel : MvxNotifyPropertyChanged
    {
        #region Properties

        public IList<GenderViewModel> AvailableGenders => new List<GenderViewModel>
        {
            new GenderViewModel(null),
            new GenderViewModel(GenderDto.Male),
            new GenderViewModel(GenderDto.Female),
            new GenderViewModel(GenderDto.NonBinary),
            new GenderViewModel(GenderDto.GenderFluid),
            new GenderViewModel(GenderDto.GenderNeutral),
        };

        public string FullName => $"{GivenName} {FamilyName}";

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _registeredOn;
        public string RegisteredOn
        {
            get => _registeredOn;
            set => SetProperty(ref _registeredOn, value);
        }

        private string _avatarImagePath;
        public string AvatarImagePath
        {
            get => _avatarImagePath;
            set => SetProperty(ref _avatarImagePath, value);
        }

        private string _familyName;
        public string FamilyName
        {
            get => _familyName;
            set => SetProperty(ref _familyName, value);
        }

        private string _givenName;
        public string GivenName
        {
            get => _givenName;
            set => SetProperty(ref _givenName, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTimeViewModel _dob;
        public DateTimeViewModel DoB
        {
            get => _dob;
            set => SetProperty(ref _dob, value);
        }

        private GenderViewModel _gender;
        public GenderViewModel Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private string _country;
        public string Country
        {
            get => _country;
            set => SetProperty(ref _country, value);
        }

        private string _telegram;
        public string Telegram
        {
            get => _telegram;
            set => SetProperty(ref _telegram, value);
        }

        private string _skype;
        public string Skype
        {
            get => _skype;
            set => SetProperty(ref _skype, value);
        }

        private string _twitter;
        public string Twitter
        {
            get => _twitter;
            set => SetProperty(ref _twitter, value);
        }

        private string _instagram;
        public string Instagram
        {
            get => _instagram;
            set => SetProperty(ref _instagram, value);
        }

        private string _facebook;
        public string Facebook
        {
            get => _facebook;
            set => SetProperty(ref _facebook, value);
        }

        private string _snapchat;
        public string Snapchat
        {
            get => _snapchat;
            set => SetProperty(ref _snapchat, value);
        }

        private string _youTube;
        public string YouTube
        {
            get => _youTube;
            set => SetProperty(ref _youTube, value);
        }

        private string _blog;
        public string Blog
        {
            get => _blog;
            set => SetProperty(ref _blog, value);
        }

        #endregion

        public void MapFromEntity(User userProfile)
        {
            if (userProfile is null)
                return;

            Username = userProfile.Username;
            RegisteredOn = userProfile.RegisteredOn.ToString("Y");
            AvatarImagePath = userProfile.AvatarPath;
            FamilyName = userProfile.FamilyName;
            GivenName = userProfile.GivenName;
            Description = userProfile.Description;
            DoB = new DateTimeViewModel(userProfile.DateOfBirth);
            Gender = new GenderViewModel(userProfile.Gender);
            Country = userProfile.Country;
            Telegram = userProfile.Telegram;
            Skype = userProfile.Skype;
            Twitter = userProfile.Twitter;
            Instagram = userProfile.Instagram;
            Facebook = userProfile.Facebook;
            Snapchat = userProfile.Snapchat;
            YouTube = userProfile.Youtube;
            Blog = userProfile.Blog;

            RaiseAllPropertiesChanged();
        }

        public void SetGender(string result)
        {
            var selectedGender = AvailableGenders.FirstOrDefault(g => string.Equals(g.LocalizedEntry, result));
            Gender = selectedGender;
        }
    }
}
