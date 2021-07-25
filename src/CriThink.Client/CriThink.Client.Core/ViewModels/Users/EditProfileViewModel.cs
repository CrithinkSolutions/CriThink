using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Builders;
using CriThink.Client.Core.Models.Identity;
using CriThink.Client.Core.Services;
using CriThink.Client.Core.ViewModels.Common;
using CriThink.Common.Endpoints.DTOs.UserProfile;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Refit;
using Xamarin.Essentials;

namespace CriThink.Client.Core.ViewModels.Users
{
    public class EditProfileViewModel : ViewModelResult<bool>, IMvxViewModelResult<EditProfileViewModelResult>
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IUserDialogs _userDialogs;
        private readonly IMvxNavigationService _navigationService;
        private readonly ILogger<EditProfileViewModel> _logger;

        private User _userProfile;
        private StreamPart _streamPart;

        public EditProfileViewModel(IUserProfileService userProfileService, IUserDialogs userDialogs, IMvxNavigationService navigationService, ILogger<EditProfileViewModel> logger)
        {
            _userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
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
            var defaultDate = UserProfileViewModel.DoBViewModel.DateTime;

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
                await _userProfileService.UpdateUserProfileAsync(request, cancellationToken);
            }
            catch (Exception)
            {
                hasFailed = true;
                message = LocalizedTextSource.GetText("UpdateError");
            }
            finally
            {
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
                await PickCustomAvatarAsync(result => ReadCustomAvatarAsync(result, cancellationToken));
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting avatar from file picker");
            }
        }

        private Task PickCustomAvatarAsync(Func<FileResult, Task> onTerminated)
        {
            var dispatcher = Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
            return dispatcher.ExecuteOnMainThreadAsync(() =>
            {
                FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Jpeg,
                    PickerTitle = LocalizedTextSource.GetText("SelectAvatar"),
                }).ContinueWith(t =>
                {
                    if (t is { IsCompleted: true })
                        onTerminated.Invoke(t.Result);
                });
            });
        }

        private async Task ReadCustomAvatarAsync(FileResult fileResult, CancellationToken cancellationToken)
        {
            if (fileResult is null ||
                !fileResult.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) &&
                !fileResult.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
                return;

            IsLoading = true;

            try
            {
                await ReadImageStreamAsync(fileResult).ConfigureAwait(true);
                await _userProfileService.UpdateUserProfileAvatarAsync(_streamPart, cancellationToken);

                UserProfileViewModel.AvatarImagePath = $"{fileResult.FullPath}";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error occurred uploading a new avatar");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ReadImageStreamAsync(FileResult result)
        {
            var stream = await result.OpenReadAsync().ConfigureAwait(false);
            _streamPart = new StreamPart(stream, result.FileName, result.ContentType);
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
    }
}
