using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CriThink.Client.Core.Services;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserProfileService _userProfileService;
        private readonly IMvxLog _log;

        public ProfileViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IUserProfileService userProfileService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
            _log = logProvider?.GetLogFor<ProfileViewModel>();

            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        private string _avatarImagePath;
        public string AvatarImagePath
        {
            get => _avatarImagePath;
            set => SetProperty(ref _avatarImagePath, value);
        }

        public List<ITransformation> ProfileImageTransformations { get; }

        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }

        private IMvxAsyncCommand _navigateToEditCommand;
        public IMvxAsyncCommand NavigateToEditCommand => _navigateToEditCommand ??= new MvxAsyncCommand(DoNavigateToEditCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _log?.Info("User navigates to profile view");
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            var user = await _userProfileService.GetUserProfileAsync().ConfigureAwait(false);
            if (user != null)
            {
                HeaderText = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("Hello"), user.Username);
                AvatarImagePath = user.AvatarPath;
            }
        }

        private async Task DoNavigateToEditCommand(CancellationToken cancellationToken)
        {
            //await _navigationService.Navigate<EditProfileViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);
        }
    }
}
