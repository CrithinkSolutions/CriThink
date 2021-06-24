using System;
using CriThink.Client.Core.ViewModels.Common;
using CriThink.Common.Endpoints.DTOs.UserProfile;

namespace CriThink.Client.Core.Builders
{
    internal class UserProfileRequestBuilder
    {
        private readonly UserProfileViewModel _viewModel;

        public UserProfileRequestBuilder(UserProfileViewModel viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public UserProfileUpdateRequest BuildUpdateRequest()
        {
            var request = new UserProfileUpdateRequest
            {
                GivenName = _viewModel.GivenName,
                FamilyName = _viewModel.FamilyName,
                Description = _viewModel.Description,
                Gender = _viewModel.Gender.Gender,
                Country = _viewModel.Country,
                Telegram = _viewModel.Telegram,
                Skype = _viewModel.Skype,
                Twitter = _viewModel.Twitter,
                Instagram = _viewModel.Instagram,
                Facebook = _viewModel.Facebook,
                Snapchat = _viewModel.Snapchat,
                Youtube = _viewModel.YouTube,
                Blog = _viewModel.Blog,
                DateOfBirth = _viewModel.DoB.DateTime
            };

            return request;
        }
    }
}
