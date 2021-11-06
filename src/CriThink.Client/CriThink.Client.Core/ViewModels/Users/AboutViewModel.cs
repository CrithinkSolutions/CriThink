using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Models.Menu;
using CriThink.Client.Core.Platform;
using CriThink.Client.Core.Services;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class AboutViewModel : BaseBottomViewViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IApplicationService _applicationService;
        private readonly IUserProfileService _userProfileService;
        private readonly IPlatformDetails _platformDetails;
        private readonly IUserDialogs _userDialogs;
        private readonly ILogger<AboutViewModel> _logger;

        private bool _isInitialized;

        public AboutViewModel(
            ILogger<AboutViewModel> logger,
            IIdentityService identityService,
            IApplicationService applicationService,
            IMvxNavigationService navigationService,
            IUserProfileService userProfileService,
            IPlatformDetails platformDetails,
            IUserDialogs userDialogs)
            : base(logger, navigationService)
        {
            _identityService = identityService ??
                throw new ArgumentNullException(nameof(identityService));

            _applicationService = applicationService ??
                throw new ArgumentNullException(nameof(applicationService));

            _userProfileService = userProfileService ??
                throw new ArgumentNullException(nameof(userProfileService));

            _platformDetails = platformDetails ??
                throw new ArgumentNullException(nameof(platformDetails));

            _userDialogs = userDialogs ??
                throw new ArgumentNullException(nameof(userDialogs));

            _logger = logger;

            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };

            MenuCollection = new MvxObservableCollection<BaseMenuItem>
            {
                new MenuModel(
                    LocalizedTextSource.GetText("Notifications"),
                    "ic_notification_settings"),

                new MenuModel(
                    LocalizedTextSource.GetText("GiveFeedback"),
                    "ic_give_us_feedback",
                    GiveFeedbackCommand),

                new MenuModel(
                    LocalizedTextSource.GetText("ToS"),
                    "ic_terms_of_service"),

                new MenuModel(
                    LocalizedTextSource.GetText("HowWorks"),
                    "ic_how_crithink_works"),

                new AccentMenuModel(
                    LocalizedTextSource.GetText("Logout"),
                    "ic_logout",
                    LogoutCommand),
            };

            TabId = "profile";
        }

        #region Properties

        private string _avatarImagePath;
        public string AvatarImagePath
        {
            get => _avatarImagePath;
            set => SetProperty(ref _avatarImagePath, value);
        }

        public MvxObservableCollection<BaseMenuItem> MenuCollection { get; }

        public List<ITransformation> ProfileImageTransformations { get; }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _navigateToProfileCommand;
        public IMvxAsyncCommand NavigateToProfileCommand => _navigateToProfileCommand ??= new MvxAsyncCommand(DoNavigateToProfile);

        private IMvxAsyncCommand _giveFeedbackCommand;
        public IMvxAsyncCommand GiveFeedbackCommand => _giveFeedbackCommand ??= new MvxAsyncCommand(DoFeedbackCommand);

        private IMvxAsyncCommand _logoutCommand;
        public IMvxAsyncCommand LogoutCommand => _logoutCommand ??= new MvxAsyncCommand(DoLogoutCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _logger?.LogInformation("User navigates to about view");

            if (!_isInitialized)
            {
                VersionTracking.Track();
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            ReadAppVersion();

            _isInitialized = true;
            var user = await _userProfileService.GetUserProfileAsync().ConfigureAwait(false);
            if (user != null)
            {
                Username = user.Username;
                AvatarImagePath = user.AvatarPath;
            }
        }

        private async Task DoNavigateToProfile(CancellationToken cancellationToken)
        {
            await NavigationService.Navigate<ProfileViewModel>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        private async Task DoFeedbackCommand(CancellationToken cancellationToken)
        {
            await _applicationService.AskForStoreReviewAsync();
        }

        private async Task DoLogoutCommand(CancellationToken cancellationToken)
        {
            var cancelText = LocalizedTextSource.GetText("LogoutCancel");
            var ok = LocalizedTextSource.GetText("LogoutOk");
            var message = LocalizedTextSource.GetText("LogoutMessage");
            var isConfirmed = await _userDialogs.ConfirmAsync(message, null, ok, cancelText, cancelToken: cancellationToken).ConfigureAwait(true);
            if (!isConfirmed)
                return;

            try
            {
                await _platformDetails.LogoutSocialLoginAsync().ConfigureAwait(false);
                _identityService.PerformLogout();

                await NavigationService.Navigate<SignUpViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    }),
                    cancellationToken: cancellationToken).ConfigureAwait(true);
            }
            catch
            {
                var errorLogoutOk = LocalizedTextSource.GetText("LogoutErrorOk");
                var errorLogoutMessage = LocalizedTextSource.GetText("LogoutErrorMessage");
                await _userDialogs.AlertAsync(errorLogoutMessage, null, errorLogoutOk, cancelToken: cancellationToken).ConfigureAwait(false);
            }
        }

        private void ReadAppVersion()
        {
            if (_isInitialized)
                return;

            var version = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("Version"), VersionTracking.CurrentVersion);

            MenuCollection.Add(new MenuModel(version, "ic_version"));
            MenuCollection.Add(new FooterModel());
        }
    }
}
