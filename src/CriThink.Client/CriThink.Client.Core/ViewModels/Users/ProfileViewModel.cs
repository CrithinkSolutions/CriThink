using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Common;
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
        private readonly IPlatformService _platformService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxLog _log;

        public ProfileViewModel(
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService,
            IUserProfileService userProfileService,
            IPlatformService platformService,
            IUserDialogs userDialogs)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
            _platformService = platformService ?? throw new ArgumentNullException(nameof(platformService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _log = logProvider?.GetLogFor<ProfileViewModel>();

            UserProfileViewModel = new UserProfileViewModel();
            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        public List<ITransformation> ProfileImageTransformations { get; }

        public string UserFullNameFormat => $"{LocalizedTextSource.GetText("MyNameIs")} {UserProfileViewModel.FullName}";

        public string UserGenderFormat => $"{LocalizedTextSource.GetText("IAmGender")} {UserProfileViewModel.Gender.LocalizedEntry}";

        public string UserCountryFormat => $"{LocalizedTextSource.GetText("ILiveIn")} {UserProfileViewModel.Country}";

        public string UserDoBFormat => $"{LocalizedTextSource.GetText("IBornOn")} {UserProfileViewModel.DoB}";

        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }

        private string _registeredOn;
        public string RegisteredOn
        {
            get => _registeredOn;
            set => SetProperty(ref _registeredOn, value);
        }

        private UserProfileViewModel _userProfileViewModel;
        public UserProfileViewModel UserProfileViewModel
        {
            get => _userProfileViewModel;
            set
            {
                SetProperty(ref _userProfileViewModel, value);
                RaiseAllPropertiesChanged();
            }
        }

        #endregion

        #region Commands

        private IMvxAsyncCommand _navigateToEditCommand;
        public IMvxAsyncCommand NavigateToEditCommand => _navigateToEditCommand ??= new MvxAsyncCommand(DoNavigateToEditCommand);

        private IMvxCommand _openTelegramCommand;
        public IMvxCommand OpenTelegramCommand => _openTelegramCommand ??= new MvxCommand(DoOpenTelegramCommand);

        private IMvxCommand _openSkypeCommand;
        public IMvxCommand OpenSkypeCommand => _openSkypeCommand ??= new MvxCommand(DoOpenSkypeCommand);

        private IMvxCommand _openFacebookCommand;
        public IMvxCommand OpenFacebookCommand => _openFacebookCommand ??= new MvxCommand(DoOpenFacebookCommand);

        private IMvxCommand _openInstagramCommand;
        public IMvxCommand OpenInstagramCommand => _openInstagramCommand ??= new MvxCommand(DoOpenInstagramCommand);

        private IMvxCommand _openTwitterCommand;
        public IMvxCommand OpenTwitterCommand => _openTwitterCommand ??= new MvxCommand(DoOpenTwitterCommand);

        private IMvxCommand _openSnapchatCommand;
        public IMvxCommand OpenSnapchatCommand => _openSnapchatCommand ??= new MvxCommand(DoOpenSnapchatCommand);

        private IMvxCommand _openYoutubeCommand;
        public IMvxCommand OpenYoutubeCommand => _openYoutubeCommand ??= new MvxCommand(DoOpenYoutubeCommand);

        private IMvxCommand _openBlogCommand;
        public IMvxCommand OpenBlogCommand => _openBlogCommand ??= new MvxCommand(DoOpenBlogCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _log?.Info("User navigates to profile view");
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            await GetUserProfileAsync().ConfigureAwait(false);
        }

        private async Task DoNavigateToEditCommand(CancellationToken cancellationToken)
        {
            var hasBeenEdited = await _navigationService.Navigate<EditProfileViewModel, bool>(cancellationToken: cancellationToken);

            if (hasBeenEdited)
                await GetUserProfileAsync();
        }

        private void DoOpenTelegramCommand()
        {
            ShowToastForSocial("Telegram", _userProfileViewModel.Telegram, async () =>
            {
                var telegramUri = new Uri($"https://t.me/{_userProfileViewModel.Telegram}");
                await LaunchInternalBrowserAsync(telegramUri);
            });
        }

        private void DoOpenSkypeCommand()
        {
            ShowToastForSocial("Skype", _userProfileViewModel.Skype, () =>
            {
                _platformService.OpenSkype(_userProfileViewModel.Skype);
            });
        }

        private void DoOpenFacebookCommand()
        {
            ShowToastForSocial("Facebook", _userProfileViewModel.Facebook, async () =>
            {
                var uri = new Uri($"https://facebook.com/{_userProfileViewModel.Facebook}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenInstagramCommand()
        {
            ShowToastForSocial("Instagram", _userProfileViewModel.Instagram, async () =>
            {
                var uri = new Uri($"https://instagram.com/{_userProfileViewModel.Instagram}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenTwitterCommand()
        {
            ShowToastForSocial("Twitter", _userProfileViewModel.Twitter, async () =>
            {
                var uri = new Uri($"https://twitter.com/{_userProfileViewModel.Twitter}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenSnapchatCommand()
        {
            ShowToastForSocial("Snapchat", _userProfileViewModel.Snapchat, async () =>
            {
                var uri = new Uri($"https://story.snapchat.com/u/{_userProfileViewModel.Snapchat}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenYoutubeCommand()
        {
            ShowToastForSocial("YouTube", _userProfileViewModel.YouTube, async () =>
            {
                var uri = new Uri($"https://www.youtube.com/c/{_userProfileViewModel.YouTube}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenBlogCommand()
        {
            ShowToastForSocial("Blog", _userProfileViewModel.Blog, async () =>
            {
                try
                {
                    var uri = new Uri(_userProfileViewModel.Blog);
                    await LaunchInternalBrowserAsync(uri);
                }
                catch (UriFormatException ex)
                {
                    _log?.WarnException("The user blog uri is malformed", ex);
                }
            });
        }

        private async Task GetUserProfileAsync()
        {
            var userProfile = await _userProfileService.GetUserProfileAsync().ConfigureAwait(false);
            if (userProfile != null)
            {
                UserProfileViewModel.MapFromEntity(userProfile);
                HeaderText = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("Hello"), UserProfileViewModel.Username);
                RegisteredOn = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("RegisteredOn"), _userProfileViewModel.RegisteredOn);

                await RaiseAllPropertiesChanged();
            }
        }

        private Task LaunchInternalBrowserAsync(Uri uri) =>
            _platformService.OpenInternalBrowser(uri);

        private void ShowToastForSocial(string socialName, string socialUsername, Action onTap)
        {
            var cfg = new ToastConfig($"{socialName}: {socialUsername}")
            {
                Action = new ToastAction
                {
                    Action = onTap,
                    Text = LocalizedTextSource.GetText("OpenApp"),
                    TextColor = Color.Yellow,
                },
                Duration = TimeSpan.FromSeconds(8),
                Icon = $"ic_{socialName.ToLowerInvariant()}_logo.png",
            };

            _userDialogs.Toast(cfg);
        }
    }
}
