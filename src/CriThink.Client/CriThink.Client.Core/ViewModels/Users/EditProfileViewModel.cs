using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Builders;
using CriThink.Client.Core.Messenger;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Common;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using FFImageLoading;
using FFImageLoading.Cache;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Refit;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class EditProfileViewModel
        : ViewModelResult<bool>,
        IMvxViewModelResult<EditProfileViewModelResult>,
        IDisposable
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxNavigationService _navigationService;
        private readonly IApplicationService _applicationService;
        private readonly MvxSubscriptionToken _token;
        private readonly ILogger<EditProfileViewModel> _logger;

        private User _userProfile;
        private StreamPart _streamPart;
        private bool _disposedValue;

        public EditProfileViewModel(
            IUserProfileService userProfileService,
            IUserDialogs userDialogs,
            IMvxNavigationService navigationService,
            IApplicationService applicationService,
            IMvxMessenger messenger,
            ILogger<EditProfileViewModel> logger)
        {
            _userProfileService = userProfileService ??
                throw new ArgumentNullException(nameof(userProfileService));

            _userDialogs = userDialogs ??
                throw new ArgumentNullException(nameof(userDialogs));

            _navigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            _applicationService = applicationService ??
                throw new ArgumentNullException(nameof(applicationService));

            _token = messenger?.Subscribe<PictureMessage>(OnImageSelected, MvxReference.Weak) ??
                throw new ArgumentNullException(nameof(messenger));

            _logger = logger;

            LogoImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };
        }

        #region Properties

        public List<ITransformation> LogoImageTransformations { get; }

        private UserProfileViewModel _userProfileViewModel;
        public UserProfileViewModel UserProfileViewModel
        {
            get => _userProfileViewModel;
            set => SetProperty(ref _userProfileViewModel, value);
        }

        private IMvxAsyncCommand _selectDobCommand;
        public IMvxAsyncCommand SelectDobCommand => _selectDobCommand ??= new MvxAsyncCommand(DoSelectDobCommand);

        private IMvxAsyncCommand _selectGenderCommand;
        public IMvxAsyncCommand SelectGenderCommand => _selectGenderCommand ??= new MvxAsyncCommand(DoSelectGenderCommand);

        private IMvxAsyncCommand _saveCommand;
        public IMvxAsyncCommand SaveCommand => _saveCommand ??= new MvxAsyncCommand(DoSaveCommand);

        private IMvxAsyncCommand _pickUpImageCommand;
        public IMvxAsyncCommand PickUpImageCommand => _pickUpImageCommand ??= new MvxAsyncCommand(DoPickUpImageCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            UserProfileViewModel = new UserProfileViewModel();
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            _userProfile = await _userProfileService.GetUserProfileAsync();
            UserProfileViewModel.MapFromEntity(_userProfile);
        }

        private async Task DoSelectDobCommand(CancellationToken cancellationToken)
        {
            var defaultDate = UserProfileViewModel.DoBViewModel?.DateTime;

            var result = await _userDialogs.DatePromptAsync(null, defaultDate, cancellationToken);
            if (result.Ok)
            {
                UserProfileViewModel.DoBViewModel = new DateTimeViewModel(result.SelectedDate);
            }
        }

        private async Task DoSelectGenderCommand(CancellationToken cancellationToken)
        {
            var cancelText = LocalizedTextSource.GetText("Cancel");

            var result = await _userDialogs.ActionSheetAsync(
                LocalizedTextSource.GetText("Gender"),
                cancelText,
                null,
                cancellationToken,
                UserProfileViewModel.AvailableGenders.Select(g => g.LocalizedEntry).ToArray());

            if (cancelText.Equals(result))
                return;

            UserProfileViewModel.SetGender(result);
        }

        private async Task DoSaveCommand(CancellationToken cancellationToken)
        {
            string message = LocalizedTextSource.GetText("UpdateOk");

            var hasFailed = false;

            var request = BuildRequest();

            try
            {
                IsLoading = true;

                await _userProfileService.UpdateUserProfileAsync(request, cancellationToken);

                await FFImageLoading.ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);
            }
            catch (Exception)
            {
                hasFailed = true;
                message = LocalizedTextSource.GetText("UpdateError");
            }
            finally
            {
                IsLoading = false;

                await ShowMessage(message, cancellationToken);

                if (!hasFailed)
                {
                    await _navigationService.Close(this, new EditProfileViewModelResult(true), cancellationToken);
                }
            }
        }

        private async Task DoPickUpImageCommand(CancellationToken cancellationToken)
        {
            try
            {
                _applicationService.ChoosePictureFromLibraryAsync();
            }
            catch (Exception ex)
            {
                var text = LocalizedTextSource.GetText("ErrorImage");

                await ShowMessage(
                        text,
                        cancellationToken);

                _logger?.LogError(ex, "Error getting avatar from file picker");
            }
        }

        private async void OnImageSelected(PictureMessage message)
        {
            try
            {
                IsLoading = true;

                using var stream = new MemoryStream(message.Bytes);

                _streamPart = new StreamPart(stream, message.FileName + ".jpg");

                await _userProfileService.UpdateUserProfileAvatarAsync(_streamPart);

                await ImageService.Instance.InvalidateCacheEntryAsync(UserProfileViewModel.AvatarImagePath, CacheType.All, true);

                var dispatcher = Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
                await dispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    UserProfileViewModel.AvatarImagePath = UserProfileViewModel.AvatarImagePath + $"?tick={DateTime.UtcNow.Ticks}";
                });
            }
            catch (Exception ex)
            {
                var text = LocalizedTextSource.GetText("ErrorImage");

                await ShowMessage(text, default);

                _logger?.LogError(ex, "Error getting avatar from file picker");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private UserProfileUpdateRequest BuildRequest()
        {
            var request = new UserProfileRequestBuilder(UserProfileViewModel)
                .BuildUpdateRequest();

            return request;
        }

        private Task ShowMessage(string message, CancellationToken cancellationToken)
        {
            return _userDialogs.AlertAsync(message, null, cancelToken: cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _token?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
