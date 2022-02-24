using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Constants;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Common;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using FFImageLoading;
using FFImageLoading.Cache;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserProfileService _userProfileService;
        private readonly IIdentityService _identityService;
        private readonly IPlatformService _platformService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILogger<ProfileViewModel> _logger;

        public ProfileViewModel(
            ILogger<ProfileViewModel> logger,
            IMvxNavigationService navigationService,
            IUserProfileService userProfileService,
            IIdentityService identityService,
            IPlatformService platformService,
            IUserDialogs userDialogs)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _platformService = platformService ?? throw new ArgumentNullException(nameof(platformService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _logger = logger;

            UserProfileViewModel = new UserProfileViewModel();
            ProfileImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        public List<ITransformation> ProfileImageTransformations { get; }

        public string UserFullNameFormat => $"{LocalizedTextSource.GetText("MyNameIs")} {UserProfileViewModel.FullName}";

        public string UserGenderFormat => $"{LocalizedTextSource.GetText("IAmGender")} {UserProfileViewModel.GenderViewModel.LocalizedEntry}";

        public string UserCountryFormat => $"{LocalizedTextSource.GetText("ILiveIn")} {UserProfileViewModel.Country}";

        public string UserDoBFormat => $"{LocalizedTextSource.GetText("IBornOn")} {UserProfileViewModel.DoBViewModel}";

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

        private IMvxAsyncCommand _closeAccountCommand;
        public IMvxAsyncCommand CloseAccountCommand => _closeAccountCommand ??= new MvxAsyncCommand(DoCloseAccountCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _logger?.LogInformation("User navigates to profile view");
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            await GetUserProfileAsync().ConfigureAwait(false);
        }


        private async Task DoNavigateToEditCommand(CancellationToken cancellationToken)
        {
            var viewModelResult = await _navigationService.Navigate<EditProfileViewModel, EditProfileViewModelResult>(cancellationToken: cancellationToken);

            await ImageService.Instance.InvalidateCacheEntryAsync(UserProfileViewModel.AvatarImagePath, CacheType.All, true);

            if (viewModelResult?.HasBeenEdited == true)
            {
                await GetUserProfileAsync();
            }
            else
            {
                // workaround to update image - cache above doesn't work
                UserProfileViewModel.AvatarImagePath = UserProfileViewModel.AvatarImagePath + $"?ticks={DateTime.UtcNow.Ticks}";
            }
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
                    _logger?.LogWarning(ex, "The user blog uri is malformed");
                }
            });
        }

        private async Task DoCloseAccountCommand(CancellationToken cancellationToken)
        {
            var isConfirmed = await AskForDeletionConfirmationAsync(cancellationToken);
            if (!isConfirmed)
                return;

            try
            {
                var scheduledDeletionDate = await _identityService.DeleteAccountAsync(cancellationToken);

                await ShowDeletionConfirmationAsync(scheduledDeletionDate, cancellationToken);

                _identityService.PerformLogout();

                await _navigationService.Navigate<SignUpViewModel>(
                    new MvxBundle(new Dictionary<string, string>
                    {
                        {MvxBundleConstaints.ClearBackStack, ""}
                    }),
                    cancellationToken: cancellationToken).ConfigureAwait(true);
            }
            catch (Exception)
            {
                await ShowErrorMessageAsync(cancellationToken);
            }
        }

        private async Task<bool> AskForDeletionConfirmationAsync(CancellationToken cancellationToken)
        {
            var title = LocalizedTextSource.GetText("DeleteConfirmationTitle");
            var message = LocalizedTextSource.GetText("DeleteConfirmationMessage");

            return await _userDialogs.ConfirmAsync(message, title, cancelToken: cancellationToken);
        }

        private async Task ShowDeletionConfirmationAsync(UserSoftDeletionResponse response, CancellationToken cancellationToken)
        {
            var title = LocalizedTextSource.GetText("DeleteTitle");
            var message = string.Format(LocalizedTextSource.GetText("DeleteMessage"),
                response.DeletionScheduledOn.ToString("D", CultureInfo.CurrentUICulture));

            await ShowAlertAsync(message, title, cancellationToken);
        }

        private async Task ShowErrorMessageAsync(CancellationToken cancellationToken)
        {
            var title = LocalizedTextSource.GetText("DeleteErrorTitle");
            var message = LocalizedTextSource.GetText("DeleteErrorMessage");

            await ShowAlertAsync(message, title, cancellationToken);
        }

        private async Task GetUserProfileAsync()
        {
            var userProfile = await _userProfileService.GetUserProfileAsync().ConfigureAwait(false);
            if (userProfile != null)
            {
                UserProfileViewModel.MapFromEntity(userProfile);

                HeaderText = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedTextSource.GetText("Hello"),
                    UserProfileViewModel.Username);

                RegisteredOn = string.Format(
                    CultureInfo.CurrentCulture,
                    LocalizedTextSource.GetText("RegisteredOn"),
                    _userProfileViewModel.RegisteredOn);

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

        private Task ShowAlertAsync(string message, string title, CancellationToken cancellationToken) =>
            _userDialogs.AlertAsync(message, title, cancelToken: cancellationToken);
    }
}
