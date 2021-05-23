using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Common;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class EditProfileViewModel : BaseViewModel
    {
        private readonly IUserProfileService _userProfileService;

        public EditProfileViewModel(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));

            LogoImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        public List<ITransformation> LogoImageTransformations { get; }

        private UserProfileViewModel _userProfile;
        public UserProfileViewModel UserProfile
        {
            get => _userProfile;
            set => SetProperty(ref _userProfile, value);
        }

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            UserProfile = new UserProfileViewModel();
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            var userProfile = await _userProfileService.GetUserProfileAsync().ConfigureAwait(true);
            UserProfile.MapFromEntity(userProfile);
        }
    }
}
