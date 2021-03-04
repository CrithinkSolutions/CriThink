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
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class AboutViewModel : BaseBottomViewViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IPlatformDetails _platformDetails;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxLog _log;

        private bool _isInitialized;

        public AboutViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IIdentityService identityService, IPlatformDetails platformDetails, IUserDialogs userDialogs)
            : base(logProvider, navigationService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _platformDetails = platformDetails ?? throw new ArgumentNullException(nameof(platformDetails));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _log = LogProvider?.GetLogFor<AboutViewModel>();

            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };

            MenuCollection = new MvxObservableCollection<BaseMenuItem>
            {
                new HeaderModel(LocalizedTextSource.GetText("AccountSettings")),
                new MenuModel(LocalizedTextSource.GetText("PersonalInfo")),
                new MenuModel(LocalizedTextSource.GetText("Notifications")),

                new HeaderModel(LocalizedTextSource.GetText("Referrals")),
                new MenuModel(LocalizedTextSource.GetText("InviteFriends")),
                new HeaderModel(LocalizedTextSource.GetText("Support")),
                new MenuModel(LocalizedTextSource.GetText("HowWorks")),
                new MenuModel(LocalizedTextSource.GetText("GetHelp")),
                new MenuModel(LocalizedTextSource.GetText("GiveFeedback")),

                new HeaderModel(LocalizedTextSource.GetText("Legal")),
                new MenuModel(LocalizedTextSource.GetText("ToS")),
                new MenuModel(LocalizedTextSource.GetText("PrivacySettings")),

                new ActionModel(LocalizedTextSource.GetText("Logout"), new MvxAsyncCommand(DoLogoutCommand))
            };

            TabId = "profile";
        }

        #region Properties

        // TODO: real pic
        public string ProfileImagePath => "res:ic_text_logo";

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

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _log?.Info("User navigates to about view");

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
            var user = await _identityService.GetLoggedUserAsync().ConfigureAwait(false);
            if (user != null)
            {
                Username = user.UserName;
            }
        }

        private async Task DoNavigateToProfile(CancellationToken cancellationToken)
        {
            await NavigationService.Navigate<ProfileViewModel>(cancellationToken: cancellationToken).ConfigureAwait(false);
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
            MenuCollection.Add(new VersionModel(version));
        }
    }
}
