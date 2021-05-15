using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Models.Identity;
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

            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        public List<ITransformation> ProfileImageTransformations { get; }

        public string UserFullNameFormat => $"{LocalizedTextSource.GetText("MyNameIs")} {_user.GivenName} {_user.FamilyName}";

        public string UserGenderFormat => $"{LocalizedTextSource.GetText("IAmGender")} {_user.Gender}";

        public string UserCountryFormat => $"{LocalizedTextSource.GetText("ILiveIn")} {_user.Country}";

        public string UserDoBFormat => $"{LocalizedTextSource.GetText("IBornOn")} {_user.DateOfBirth?.ToString("D")}";

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

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                SetProperty(ref _user, value);
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

            User = await _userProfileService.GetUserProfileAsync().ConfigureAwait(false);
            if (User != null)
            {
                HeaderText = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("Hello"), _user.Username);
                RegisteredOn = string.Format(CultureInfo.CurrentCulture, LocalizedTextSource.GetText("RegisteredOn"), _user.RegisteredOn.ToString("Y"));
            }
        }

        private async Task DoNavigateToEditCommand(CancellationToken cancellationToken)
        {
            //await _navigationService.Navigate<EditProfileViewModel>(cancellationToken: cancellationToken).ConfigureAwait(true);
        }

        private void DoOpenTelegramCommand()
        {
            ShowToastForSocial("Telegram", _user.Telegram, async () =>
            {
                var telegramUri = new Uri($"https://t.me/{_user.Telegram}");
                await LaunchInternalBrowserAsync(telegramUri);
            });
        }

        private void DoOpenSkypeCommand()
        {
            ShowToastForSocial("Skype", _user.Skype, () =>
            {
                _platformService.OpenSkype(_user.Skype);
            });
        }

        private void DoOpenFacebookCommand()
        {
            ShowToastForSocial("Facebook", _user.Facebook, async () =>
            {
                var uri = new Uri($"https://facebook.com/{_user.Facebook}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenInstagramCommand()
        {
            ShowToastForSocial("Instagram", _user.Instagram, async () =>
            {
                var uri = new Uri($"https://instagram.com/{_user.Instagram}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenTwitterCommand()
        {
            ShowToastForSocial("Twitter", _user.Twitter, async () =>
            {
                var uri = new Uri($"https://twitter.com/{_user.Twitter}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenSnapchatCommand()
        {
            ShowToastForSocial("Snapchat", _user.Snapchat, async () =>
            {
                var uri = new Uri($"https://story.snapchat.com/u/{_user.Snapchat}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenYoutubeCommand()
        {
            ShowToastForSocial("Youtube", _user.Youtube, async () =>
            {
                var uri = new Uri($"https://www.youtube.com/c/{_user.Youtube}");
                await LaunchInternalBrowserAsync(uri);
            });
        }

        private void DoOpenBlogCommand()
        {
            ShowToastForSocial("Blog", _user.Blog, async () =>
            {
                try
                {
                    var uri = new Uri(_user.Blog);
                    await LaunchInternalBrowserAsync(uri);
                }
                catch (UriFormatException ex)
                {
                    _log?.WarnException("The user blog uri is malformed", ex);
                }
            });
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
