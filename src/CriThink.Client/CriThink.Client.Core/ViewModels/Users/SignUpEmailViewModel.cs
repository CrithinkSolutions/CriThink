using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CriThink.Client.Core.Exceptions;
using CriThink.Client.Core.Services;
using CriThink.Common.Endpoints.DTOs.IdentityProvider;
using CriThink.Common.Helpers;
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
    public class SignUpEmailViewModel : BaseViewModel, IDisposable
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IIdentityService _identityService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILogger<SignUpEmailViewModel> _logger;

        private StreamPart _streamPart;
        private bool _isUsernameAvailable;
        private bool _isUsernameChecked;
        private CancellationTokenSource _usernameCancellationToken;

        public SignUpEmailViewModel(
            IMvxNavigationService navigationService,
            IIdentityService identityService,
            IUserDialogs userDialogs,
            ILogger<SignUpEmailViewModel> logger)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _userDialogs = userDialogs ?? throw new ArgumentNullException(nameof(userDialogs));
            _logger = logger;

            AvatarImageTransformations = new List<ITransformation>
            {
                new CircleTransformation()
            };

            UsernameAvailabilityCommand = new MvxCommand(() =>
                UsernameAvailabilityTaskNotifier = MvxNotifyTask.Create(() =>
                    CheckForUsernameAvailabilityAsync(),
                    ex => OnUsernameAvailabilityException(ex)));
        }

        private void OnUsernameAvailabilityException(Exception _)
        {
            _isUsernameAvailable = false;
            _isUsernameChecked = false;
            IsLoading = false;
        }

        private async Task CheckForUsernameAvailabilityAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_username) ||
                    _username.Length <= 2)
                {
                    _isUsernameChecked = false;
                    return;
                }

                IsLoading = true;

                if (_usernameCancellationToken is not null)
                    _usernameCancellationToken.Cancel();

                _usernameCancellationToken = new CancellationTokenSource();
                _usernameCancellationToken.Token.ThrowIfCancellationRequested();

                await Task.Delay(300);

                _isUsernameAvailable = await _identityService.CheckForUsernameAvailabilityAsync(
                    _username,
                    _usernameCancellationToken.Token);

                _isUsernameChecked = true;
            }
            finally
            {
                await RaisePropertyChanged(nameof(UsernameAvailableText));
                await RaisePropertyChanged(nameof(UsernameUnvailableText));
                await RaisePropertyChanged(nameof(IsUsernameUnavailable));
                await RaisePropertyChanged(nameof(IsUsernameAvailable));
                IsLoading = false;
            }
        }

        #region Properties

        public List<ITransformation> AvatarImageTransformations { get; }

        public string UsernameAvailableText =>
            string.Format(LocalizedTextSource.GetText("UsernameAvailable"), Username);

        public string UsernameUnvailableText =>
            string.Format(LocalizedTextSource.GetText("UsernameUnavailable"), Username);

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                RaisePropertyChanged(() => SignUpCommand);
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                RaisePropertyChanged(() => SignUpCommand);
                UsernameAvailabilityCommand.Execute(null);
            }
        }

        public bool IsUsernameAvailable => _isUsernameAvailable && _isUsernameChecked;

        public bool IsUsernameUnavailable => !_isUsernameAvailable && _isUsernameChecked;

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RaisePropertyChanged(() => SignUpCommand);
            }
        }

        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                SetProperty(ref _repeatPassword, value);
                RaisePropertyChanged(() => SignUpCommand);
            }
        }

        private string _customImagePath;
        public string CustomImagePath
        {
            get => _customImagePath;
            set => SetProperty(ref _customImagePath, value);
        }

        #endregion

        #region Commands

        public IMvxCommand UsernameAvailabilityCommand { get; private set; }

        private MvxNotifyTask _myTaskNotifier;
        public MvxNotifyTask UsernameAvailabilityTaskNotifier
        {
            get => _myTaskNotifier;
            private set => SetProperty(ref _myTaskNotifier, value);
        }

        private IMvxAsyncCommand _signUpCommand;
        public IMvxAsyncCommand SignUpCommand => _signUpCommand ??= new MvxAsyncCommand(DoSignUpCommand, () =>
            !IsLoading &&
            _isUsernameAvailable &&
            !string.IsNullOrWhiteSpace(Email) && EmailHelper.IsEmail(Email) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            string.Equals(Password, RepeatPassword, StringComparison.CurrentCulture));

        private IMvxAsyncCommand _pickUpImageCommand;
        private bool _disposedValue;

        public IMvxAsyncCommand PickUpImageCommand => _pickUpImageCommand ??= new MvxAsyncCommand(DoPickUpImageCommand);

        #endregion

        public override void Prepare()
        {
            base.Prepare();
            _logger?.LogInformation("User navigates to sign up with email");
        }

        private async Task DoPickUpImageCommand()
        {
            try
            {
                await PickCustomAvatarAsync(ReadCustomAvatarAsync);
            }
            catch (TaskCanceledException) { }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting avatar from file picker");
            }
        }

        private async Task DoSignUpCommand(CancellationToken cancellationToken)
        {
            IsLoading = true;

            var request = new UserSignUpRequest
            {
                Password = Password,
                Email = Email,
                Username = Username
            };

            try
            {
                var userInfo = await _identityService.PerformSignUpAsync(request, _streamPart, cancellationToken)
                    .ConfigureAwait(false);

                if (userInfo is null)
                {
                    var localizedText = LocalizedTextSource.GetText("ErrorMessage");
                    await ShowMessageAsync(localizedText).ConfigureAwait(true);
                }
                else
                {
                    var localizedText = LocalizedTextSource.GetText("WelcomeMessage");
                    await ShowMessageAsync(localizedText).ConfigureAwait(true);

                    await _navigationService.Navigate<LoginViewModel>(cancellationToken: cancellationToken)
                        .ConfigureAwait(true);
                }
            }
            catch (CriThinkSignUpException ex)
            {
                var errors = ex.GetErrorList();
                await ShowMessageAsync(errors);
            }
            finally
            {
                IsLoading = false;
                await RaisePropertyChanged(() => SignUpCommand);
            }
        }

        public async Task ConfirmUserEmailAsync(string userId, string code)
        {
            IsLoading = true;

            try
            {
                var userInfo = await _identityService.ConfirmUserEmailAsync(userId, code).ConfigureAwait(false);
                if (userInfo == null)
                {
                    var localizedText = LocalizedTextSource.GetText("ConfirmErrorMessage");
                    await ShowMessageAsync(localizedText).ConfigureAwait(true);
                }
                else
                {
                    var localizedText = LocalizedTextSource.GetText("ConfirmWelcomeMessage");
                    await ShowMessageAsync(string.Format(CultureInfo.CurrentCulture, localizedText, userInfo.Username)).ConfigureAwait(true);
                    await _navigationService.Navigate<LoginViewModel>().ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error confirming user email");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ReadImageStreamAsync(FileResult result)
        {
            var stream = await result.OpenReadAsync().ConfigureAwait(false);
            ReadStreamFromFile(stream, result.FileName, result.ContentType);
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

        private async Task ReadCustomAvatarAsync(FileResult fileResult)
        {
            if (fileResult is null ||
                !fileResult.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) &&
                !fileResult.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
                return;

            CustomImagePath = $"{fileResult.FullPath}";
            await ReadImageStreamAsync(fileResult).ConfigureAwait(true);
        }

        private void ReadStreamFromFile(Stream stream, string fileName, string contentType)
        {
            _streamPart = new StreamPart(stream, fileName, contentType);
        }

        private Task ShowMessageAsync(string message)
        {
            return _userDialogs.AlertAsync(
                message,
                okText: "Ok");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _usernameCancellationToken?.Dispose();
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
